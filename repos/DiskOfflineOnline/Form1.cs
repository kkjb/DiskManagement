using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


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

        // 磁盘设备列表
        private async void LoadDisks()
        {
            listBoxDisks.Items.Clear();  // 清空 ListBox 中的现有项目

            // 使用WMI查询所有磁盘
            var searcher = new System.Management.ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
            var disks = searcher.Get();

            if (disks.Count == 0)
            {
                MessageBox.Show("No disks found.");  // 如果没有找到磁盘，弹出提示
            }

            foreach (System.Management.ManagementObject disk in disks)
            {
                string diskId = disk["DeviceID"]?.ToString();  // 获取磁盘ID
                string model = disk["Model"]?.ToString();     // 获取磁盘型号
                string diskNumber = ExtractDiskNumber(diskId);
                string status = await GetDiskStatusAsync(diskNumber);  // 异步获取磁盘状态

                listBoxDisks.Items.Add($"{model} - {diskId} - {status}");  // 将磁盘添加到 ListBox 中
            }
        }


        // 获取磁盘状态（联机/脱机）并使用异步执行 PowerShell 脚本
        private async Task<string> GetDiskStatusAsync(string diskNumber)
        {
            string status = string.Empty;

            // 使用 PowerShell 获取磁盘状态
            string script = $"Get-Disk -Number {diskNumber} | Select-Object -ExpandProperty OperationalStatus";
            string operationalStatus = await ExecutePowerShellScriptAndGetOutputAsync(script);

            if (operationalStatus.Contains("Offline"))
            {
                status = "脱机";
            }
            else
            {
                status = "联机";
            }

            return status;
        }

        // 执行 PowerShell 脚本并异步获取输出
        private async Task<string> ExecutePowerShellScriptAndGetOutputAsync(string script)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo()
            {
                FileName = "powershell.exe",
                Arguments = $"-Command \"{script}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                Verb = "runas"
            };

            try
            {
                using (Process process = Process.Start(processStartInfo))
                {
                    string output = await process.StandardOutput.ReadToEndAsync();  // 使用异步方式读取标准输出
                    string error = await process.StandardError.ReadToEndAsync();    // 使用异步方式读取错误输出
                    process.WaitForExit(); // 等待进程结束

                    if (process.ExitCode != 0)
                    {
                        MessageBox.Show($"PowerShell 执行失败，退出码：{process.ExitCode}\n错误：{error}");
                    }

                    return output.Trim();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"操作失败: {ex.Message}");
                return string.Empty;
            }
        }

        private string ExtractDiskNumber(string deviceId)
        {
            // 正则表达式匹配 \\.\PHYSICALDRIVEX 格式的字符串，X为磁盘编号
            var match = Regex.Match(deviceId, @"\\\.\\PHYSICALDRIVE(\d+)");
            if (match.Success)
            {
                return match.Groups[1].Value;  // 提取并返回磁盘编号
            }
            return string.Empty;  // 如果没有匹配，返回空字符串
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

            // 提取磁盘编号
            string diskNumber = ExtractDiskNumber(selectedDisk);
            MessageBox.Show($"准备脱机磁盘\r\n型号：{selectedDisk}\r\n磁盘编号 {diskNumber}");  // 调试信息，确认磁盘编号

            // 调用 SetDiskOffline 执行磁盘脱机
            SetDiskOffline(diskNumber);
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

            string diskNumber = ExtractDiskNumber(selectedDisk);
            MessageBox.Show($"准备联机磁盘\r\n型号：{selectedDisk}\r\n磁盘编号 {diskNumber}");  // 调试信息，确认磁盘编号

            // 调用 SetDiskOnline 执行磁盘联机
            SetDiskOnline(diskNumber);
            // 执行完操作后刷新磁盘状态
            LoadDisks();  // 重新加载磁盘列表并刷新状态
        }

        // 使用 PowerShell 脚本设置磁盘脱机
        private void SetDiskOffline(string diskNumber)
        {
            try
            {
                // PowerShell 脚本内容
                string script = $"Get-Disk -Number {diskNumber} | Set-Disk -IsOffline $true";

                // 执行 PowerShell 脚本
                ExecutePowerShellScript(script);

                // 提示用户磁盘已脱机
                // MessageBox.Show($"磁盘 {diskNumber} 已成功脱机！");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"操作失败: {ex.Message}");
            }
        }

        // 使用 PowerShell 脚本设置磁盘联机
        private void SetDiskOnline(string diskNumber)
        {
            try
            {
                // PowerShell 脚本内容
                string script = $"Get-Disk -Number {diskNumber} | Set-Disk -IsOffline $false";

                // 执行 PowerShell 脚本
                ExecutePowerShellScript(script);

                // 提示用户磁盘已联机
                MessageBox.Show($"磁盘 {diskNumber} 已成功联机！");
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
