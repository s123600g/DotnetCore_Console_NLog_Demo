---
title: Dotnet Core 3.1 Console App使用NLog筆記
tags: GitHub,Dotnet Core, NLog
description: 建置.net core 3.1 console app 套入NLog範例
---

# Dotnet Core 3.1 Console App使用NLog筆記

使用 Microsoft Visual Studio Community 2019
使用 .Net Core 3.1

相關參考:
1. https://github.com/NLog/NLog
2. https://github.com/nlog/nlog/wiki/Configuration-file#rules
3. https://nlog-project.org/download/
4. https://github.com/NLog/NLog/wiki/Getting-started-with-.NET-Core-2---Console-application
5. https://github.com/NLog/NLog.Extensions.Logging/wiki/NLog-configuration-with-appsettings.json

## 操作步驟:

### Step 1. 建立 `.net core console app` 專案。

### Step 2. 對專案安裝相關套件，使用Nuget套件管理員進行。
安裝下列套件
* `NLog.Extensions.Logging`
* `Microsoft.Extensions.Configuration.FileExtensions`
* `Microsoft.Extensions.Configuration.Json`
* `Microsoft.Extensions.Configuration`

### Step 3. 建立組態設定檔
`appsettings.json` 內容
```json=
{
  "NLog": {
    "autoReload": true,
    "throwConfigExceptions": false,
    "internalLogLevel": "info",
    "internalLogFile": "${basedir}/logs/internal-nlog/internal-nlog.txt",
    "targets": {
      "logfile": {
        "type": "File",
        "fileName": "${basedir}/logs/NLogDeml${shortdate}.log",
        "layout": "${date} [${uppercase:${level}}] ${message} ${exception}${newline}"
      },
      "logconsole": {
        "type": "Console",
        "layout": "${date} [${uppercase:${level}}] ${message} ${exception}"
      }
    },
    "rules": [
      {
        "logger": "*",
        "minLevel": "Debug",
        "writeTo": "logfile,logconsole"
      }
    ]
  }
}
```
關於NLog設定參數可參考
https://nlog-project.org/config/
https://github.com/nlog/nlog/wiki/Configuration-file

**要設定紀錄輸出目的端**是在`"targets"`
在上面設定中，有兩種輸出目的端一種是寫入到記錄檔(`"File"`)，另一種是輸出至Console畫面視窗中(`"Console"`)。

* `"type"` 輸出目的類型
* `"fileName"` 如果是輸出記錄檔，跟此設置有關
* `layout` 跟輸出顯示格式有關

有關內容參數值可參考 https://nlog-project.org/config/?tab=layout-renderers

**要設定輸出規則**是在`"rules"`
* `"logger"` 可以設置紀錄內容樣式匹配，可參考 https://github.com/nlog/nlog/wiki/Configuration-file#logger-name-filter
* `"minLevel"` 設置紀錄層級最低開始點，可參考 
https://github.com/nlog/nlog/wiki/Configuration-file#logger-level-filter
* `"writeTo"` 設置規則匹配紀錄要將內容輸出至哪一個目的端，可參考 https://github.com/nlog/nlog/wiki/Configuration-file#rules

### Step 4. 完成Program.cs內容
```csharp=
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
```

### Step 5. 建置專案並執行
![](https://i.imgur.com/HiFb5vq.png)

在專案目錄`bin\Debug\netcoreapp3.1`底下可以看到`logs/`目錄，NLog產生記錄檔放置在此目錄底下
![](https://i.imgur.com/h8Fv0c2.png)


---

初步建置過程碰到下面問題，筆記解決方式
```
嚴重性	程式碼	說明	專案	檔案	行	隱藏項目狀態
錯誤 CS1061  'IConfigurationBuilder' 未包含 'AddJsonFile' 的定義，也找不到可接受類型 'IConfigurationBuilder' 第一個引數的可存取擴充方法 'AddJsonFile'(是否遺漏 using 指示詞或組件參考?)	DotnetCore_Console_NLog_Demo E:\Project\DotnetCore_Console_NLog_Demo\Program.cs  16  作用中
```
https://stackoverflow.com/a/27382878
在Nuget加入此套件 --> Microsoft.Extensions.Configuration.Json
https://www.nuget.org/packages/Microsoft.Extensions.Configuration.Json/


