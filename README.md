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
   - 資料庫已包含建立好的 `MyOffice_ACPD`、`MyOffice_ExcuteionLog` 資料表、相關預存程序（SP）以及測試資料。

2. **修改連線字串**：
   - 開啟 Web API 專案下的 `appsettings.json`。
   - 將 `ConnectionStrings:DefaultConnection` 中的 `Server` 修改為您的本機 SQL Server 執行個體名稱。

3. **啟動專案與測試**：
   - 透過 Visual Studio 2022 開啟方案，按下 `F5` 啟動專案。
   - 瀏覽器將自動開啟 Swagger 介面。
   - 每支 API 皆支援直接透過 Swagger UI 進行 CRUD 操作測試（已附帶預設的 JSON 測試模型）。

---

1. **自訂主鍵產生器 (`NEWSID` 預存程序整合)**
   - 在測試 **新增 (POST)** API 時，若 Request Body 中未填寫 `acpD_SID`，後端程式將會自動呼叫資料庫中的 `NEWSID` 預存程序，產生符合特定規則的 20 碼唯一識別碼作為主鍵寫入。

2. **系統執行記錄與例外攔截 (`usp_AddLog` 預存程序整合)**
   - 已實作完整的 `try-catch` 例外處理機制，並整合 `usp_AddLog` 預存程序。
   - **如何測試此功能**：
     請在 Swagger 中使用 **更新 (PUT)** API (`/api/MyOfficeAcpd/{id}`)，並刻意輸入一組**不存在的 ID**（例如 `99999`）。系統除了會正確回傳 `404 Not Found` 之外，還會將攔截到的 `DbUpdateConcurrencyException` 併發錯誤資訊，透過預存程序寫入到資料庫的 `MyOffice_ExcuteionLog` 資料表中。您可以直接查詢該資料表來驗證錯誤日誌紀錄。
