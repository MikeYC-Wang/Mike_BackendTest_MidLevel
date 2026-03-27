# 水星防火工程顧問 - 後端工程師技術測試

## 專案架構
- **開發框架**：.NET 8.0 Web API
- **資料庫**：SQL Server 2019+
- **ORM**：Entity Framework Core 8.0
- **架構設計**：標準 RESTful API 架構，使用 Controller 實作資源導向的 CRUD 介面。

## 執行步驟

1. **資料庫還原**：
   - 請開啟 SQL Server Management Studio (SSMS)。
   - 將根目錄下的 `BackendExamHub.bak` 檔案進行還原。
   - 資料庫已包含建立好的 `MyOffice_ACPD` 資料表以及測試資料。

2. **修改連線字串**：
   - 開啟 Web API 專案下的 `appsettings.json`。
   - 將 `ConnectionStrings:DefaultConnection` 中的 `Server` 修改為的本機 SQL Server 執行名稱。

3. **啟動專案與測試**：
   - 透過 Visual Studio 2022 開啟方案，按下 `F5` 啟動專案。
   - 瀏覽器將自動開啟 Swagger 介面。
   - 每支 API 皆支援直接透過 Swagger UI 進行 CRUD 操作測試（已附帶預設的 JSON 測試模型）。
   - **注意事項**：新增 (POST) 資料時，若未填寫 `ACPD_SID`，後端程式將會自動產生一組長度 20 的唯一識別碼作為主鍵。