using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Management;
namespace DiskOfflineOnline
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // 窗体加载时调用 LoadDisks 方法
        private void Form1_Load(object sender, EventArgs e)
        {
            LoadDisks();  // 调用 LoadDisks 方法，加载磁盘列表
        }

        /// <summary>
        /// 异步加载磁盘列表
        /// 结合 Win32_DiskDrive 和 MSFT_Disk 来获取磁盘状态（联机/离线）
        /// </summary>
        private async void LoadDisks()
        {
            listBoxDisks.Items.Clear();  // 清空 ListBox中的现有项目

            try
            {
                // 1. 查询 Win32_DiskDrive（获取磁盘基本信息）
                var win32Searcher = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
                var win32Disks = win32Searcher.Get();

                // 2. 查询 MSFT_Disk（获取硬盘在线/离线状态）
                var msftSearcher = new ManagementObjectSearcher(@"root\Microsoft\Windows\Storage", "SELECT * FROM MSFT_Disk");
                var msftDisks = msftSearcher.Get();

                if (win32Disks.Count == 0)
                {
                    MessageBox.Show("未找到任何磁盘！");
                    return;
                }

                // 3. 构建 MSFT_Disk 的字典，Key = Number，Value = IsOffline
                var msftDict = new Dictionary<uint, bool>();
                foreach (ManagementObject md in msftDisks)
                {
                    uint number = (uint)(md["Number"] ?? 0);
                    bool isOffline = (bool)(md["IsOffline"] ?? false);
                    msftDict[number] = isOffline;
                }

                // 4. 遍历 Win32_DiskDrive，按 Index 匹配 MSFT_Disk 获取 IsOffline
                foreach (ManagementObject wd in win32Disks)
                {
                    uint index = (uint)(wd["Index"] ?? 0);
                    string model = wd["Model"]?.ToString() ?? "未知型号";
                    string deviceId = wd["DeviceID"]?.ToString() ?? "未知ID";

                    // 根据 Index 查找 MSFT_Disk 的 IsOffline 状态
                    string diskIsOffline = "未知";
                    if (msftDict.ContainsKey(index))
                    {
                        diskIsOffline = msftDict[index] ? "离线" : "联机";
                    }

                    // 异步添加到 ListBox
                    await Task.Run(() =>
                    {
                        this.Invoke(new Action(() =>
                        {
                            listBoxDisks.Items.Add($"{diskIsOffline} - {model} - {deviceId} - DISK {index}");
                        }));
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载磁盘失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 从 ListBox 输出的字符串中提取磁盘编号（DISK 后面的数字）
        /// 格式示例： "联机 - SHGP31-2000GM - \\.\PHYSICALDRIVE0 - DISK 0"
        /// </summary>
        /// <param name="line">ListBox 的一行文本</param>
        /// <returns>磁盘编号，如果解析失败返回 -1</returns>
        private int ExtractDiskNumber(string line)
        {
            if (string.IsNullOrEmpty(line))
                return -1;

            // 按 "- DISK" 分割
            var parts = line.Split(new string[] { "- DISK" }, StringSplitOptions.None);
            if (parts.Length != 2)
                return -1;

            // 去掉空格并尝试解析为整数
            if (int.TryParse(parts[1].Trim(), out int diskNumber))
                return diskNumber;

            return -1;
        }

        // 刷新按钮事件
        private void Refresh_Click(object sender, EventArgs e)
        {
            
            LoadDisks();  // 重新加载磁盘列表并刷新状态
        }

        // 脱机按钮事件
        private void buttonOffline_Click(object sender, EventArgs e)
        {
            string selectedDisk = listBoxDisks.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(selectedDisk))
            {
                MessageBox.Show("请选择一个磁盘！");
                return;
            }

            // 提取 diskIndex
            string diskIndex = ExtractDiskNumber(selectedDisk).ToString();
            MessageBox.Show($"准备脱机磁盘\r\n型号：{selectedDisk}\r\n磁盘编号 {diskIndex}");  // 调试信息，确认磁盘编号

            // 调用 SetDiskOffline 执行磁盘脱机
            SetDiskOffline(diskIndex); 
            // 执行完操作后刷新磁盘状态
            LoadDisks();  // 重新加载磁盘列表并刷新状态    
        }

        // 联机按钮事件
        private void buttonOnline_Click(object sender, EventArgs e)
        {
            string selectedDisk = listBoxDisks.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(selectedDisk))
            {
                MessageBox.Show("请选择一个磁盘！");
                return;
            }

            string diskIndex = ExtractDiskNumber(selectedDisk).ToString();
            MessageBox.Show($"准备联机磁盘\r\n型号：{selectedDisk}\r\n磁盘编号 {diskIndex}");  // 调试信息，确认磁盘编号
            // 调用 SetDiskOnline 执行磁盘联机
            SetDiskOnline(diskIndex);   
            // 执行完操作后刷新磁盘状态
            LoadDisks();  // 重新加载磁盘列表并刷新状态    
        }

        // 使用 PowerShell 脚本设置磁盘脱机（根据 index 执行）
        private void SetDiskOffline(string index)
        {
            try
            {
                // PowerShell 脚本内容
                string script = $"Get-Disk -Number {index} | Set-Disk -IsOffline $true";

                // 执行 PowerShell 脚本
                ExecutePowerShellScript(script);

                // 提示用户磁盘已脱机
                // MessageBox.Show($"磁盘 {index} 已成功脱机！");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"操作失败: {ex.Message}");
            }
        }

        // 使用 PowerShell 脚本设置磁盘联机
        private void SetDiskOnline(string index)
        {
            try
            {
                // PowerShell 脚本内容
                string script = $"Get-Disk -Number {index} | Set-Disk -IsOffline $false";

                // 执行 PowerShell 脚本
                ExecutePowerShellScript(script);

                // 提示用户磁盘已联机
                // MessageBox.Show($"磁盘 {index} 已成功联机！");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"操作失败: {ex.Message}");
            }
        }

        // 执行 PowerShell 脚本
        private void ExecutePowerShellScript(string script)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo()
            {
                FileName = "powershell.exe",   // 调用 PowerShell 执行命令
                Arguments = $"-Command \"{script}\"",  // 传递脚本参数
                UseShellExecute = false,      // 不使用 shell 执行
                RedirectStandardOutput = true,  // 重定向标准输出
                RedirectStandardError = true,   // 重定向标准错误
                CreateNoWindow = true,        // 不创建命令行窗口
                Verb = "runas"                // 以管理员权限运行
            };

            try
            {
                using (Process process = Process.Start(processStartInfo))
                {
                    // 捕获输出信息
                    string output = process.StandardOutput.ReadToEnd();  // 获取标准输出
                    string error = process.StandardError.ReadToEnd();    // 获取错误输出

                    // 显示调试输出
                    Console.WriteLine("PowerShell Output:\n" + output);
                    Console.WriteLine("PowerShell Error:\n" + error);

                    // 等待命令执行完成
                    process.WaitForExit();

                    // 检查退出码
                    if (process.ExitCode != 0)
                    {
                        MessageBox.Show($"PowerShell 执行失败，退出码：{process.ExitCode}\n错误：{error}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"操作失败: {ex.Message}");
            }
        }
    }
}
