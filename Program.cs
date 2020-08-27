using Microsoft.Extensions.Configuration;
using NLog;
using NLog.Extensions.Logging;
using System;

namespace DotnetCore_Console_NLog_Demo
{
    internal class Program
    {
        private static IConfiguration config;

        private static void Main(string[] args)
        {
            #region 載入組態設定檔

            // https://stackoverflow.com/questions/27382481/why-does-visual-studio-tell-me-that-the-addjsonfile-method-is-not-defined
            // https://www.nuget.org/packages/Microsoft.Extensions.Configuration.Json/
            // 嚴重性	程式碼	說明	專案	檔案	行	隱藏項目狀態
            // 錯誤 CS1061  'IConfigurationBuilder' 未包含 'AddJsonFile' 的定義，也找不到可接受類型 'IConfigurationBuilder' 第一個引數的可存取擴充方法 'AddJsonFile'(是否遺漏 using 指示詞或組件參考?)	DotnetCore_Console_NLog_Demo E:\Project\DotnetCore_Console_NLog_Demo\Program.cs  16  作用中
            // 在Nuget加入此套件 --> Microsoft.Extensions.Configuration.Json
            // 載入appsettings json 內容
            config = new ConfigurationBuilder()
            .SetBasePath(System.IO.Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();

            // NLog configuration with appsettings.json
            // https://github.com/NLog/NLog.Extensions.Logging/wiki/NLog-configuration-with-appsettings.json
            // 從組態設定檔載入NLog設定
            NLog.LogManager.Configuration = new NLogLoggingConfiguration(config.GetSection("NLog"));
            Logger logger = LogManager.GetCurrentClassLogger();

            #endregion 載入組態設定檔

            logger.Info("-------------------- DotnetCore_Console_NLog_Demo Start --------------------");

            logger.Info("Hello World.");

            logger.Info("-------------------- DotnetCore_Console_NLog_Demo End --------------------");

            Console.ReadLine();
        }
    }
}