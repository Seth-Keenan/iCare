# iCare Documentation

## Project Overview

For the implementation of iCare, our group decided to use **ASP.NET** as the primary tech stack. We used **MS SQL Server** for our database and **HTML/CSS** for styling. The project includes the implementation of CRUD operations for both worker and admin views.

---

## Setup and Installation

### 1. Prerequisites
Ensure that you have the following installed:

- **Visual Studio** (with the settings your professor requested)
- **MS SQL Server**  
  - Create a new database called `Group2_iCAREDB`

### 2. Visual Studio Setup

1. Open Visual Studio and ensure the following components are installed:
   - **.NET Framework and Item Templates**
   
2. Create a new **ASP.NET Web Application**.

3. Inside the project folder, there is a file called `DBQuery.txt`. 
   - Open this file and copy its contents.

4. In **SQL Server Management Studio**:
   - Right-click on your `Group2_iCAREDB` database and select **New Query**.
   - Paste the contents of `DBQuery.txt` and click **Execute**.

5. In **Visual Studio**:
   - Open the `web.config` file.
   - Locate the `<connectionStrings>` tag and ensure it contains only one connection string.
     - **Data Source** should be set to your local machine's desktop name. (You can find this by searching for “About Me” in Windows and copying the desktop name.)

6. Go to **Tools** → **Connect to Database**:
   - Enter your local desktop name for the **server name**.
   - Select `Group2_iCAREDB` from the dropdown list and click **Finish**.
   - Ensure **Trust Server Certificate** is checked.

7. If you encounter any errors, go to the **Models** folder, open `Entities.edmx`, right-click and select **Update Model from Database**.

8. In **Server Explorer**, right-click the database and ensure the connection is green and connected.

9. Click **Build** to ensure there are no database connection issues. If the build fails, double-check the connection string and database name.

**Note:** Always run the project while hovering over the **Controller** folder (not Views or Models).

---

## Navigation

### Important Notes:
- It’s recommended to create **geocodes**, **workers**, and **roles** before starting operations. These can be found in the **Admin Dashboard**.
- This will help you avoid logging in and out frequently due to the two different account dashboards (Admin and Worker).

---

## Admin and User Login

The provided query includes the following default credentials:

- **Admin Login**:
  - Username: `ADMIN`
  - Password: `ADMIN`
  
- **Doctor Login**:
  - Username: `DOCTOR`
  - Password: `DOCTOR`

- **Nurse Login**:
  - Username: `NURSE`
  - Password: `NURSE`

Upon login, users will be redirected to their respective dashboards (Admin or Worker), where they can perform various operations.

---

## Manage and Register Users with Different Roles

1. In the **Admin Dashboard**, there is a card called **iCARE User Roles**.
2. Create the roles you need and submit the form.
3. Navigate to the **Manage Accounts** card to perform CRUD operations on accounts with the roles you've created.

---

## Entry and Managing Patient Records

1. **Worker Login** provides access to the **Manage Patient Records** card.
2. Here, you can perform CRUD operations on patient records.
3. Ensure that the patient ID is unique.
4. After filling out the fields, the **Next** button will allow you to upload additional files (PDFs) to the project’s repository folder.

---

## Display Patients and Self-Assigning

1. The **Assign Patient** card displays all patients in the **Worker Dashboard**.
2. You can assign patients by selecting the checkboxes on the left and clicking **Assign Selected Patient**.
3. This action assigns the patient to the currently logged-in worker.

---

## Treating and Updating Patient Records

1. To update a patient's record, go to **Manage Patient Records** in the **Worker Dashboard**.
2. You can edit or delete patient records from here.
   - **Edit** allows for information changes.
   - **Delete** removes the patient record entirely.

---

## Display Pallet

1. Once patients are assigned, their documentation will be shown on the **Display Pallet**.
2. There is a button that allows you to redirect to **Manage Documents**, where you can modify patient documents.
3. **Manage PDFs** allows modifications to patient PDFs, and **Manage Other Documents** lets you modify non-PDF files uploaded for the patient.

---

## Display My Board

1. Assigned patients will appear on the **Display My Board** card in the **Worker Dashboard**.
2. This provides a quick view of the currently assigned patients.

---

## Manage Documents (Including Images)

1. From the **Display Pallet**, there is a button to manage documents.
2. This allows you to insert, edit, or delete documents associated with patients.
3. You can upload and manage image files (e.g., medical images).

---

## Integration with External Drug System

1. In the **Manage Document** card, after a worker is created, click on **Create New** to add a new document.
2. While filling in the **Description** field, drug names will auto-suggest as you type (e.g., try typing “Ty”, “Be”, or “As”).
3. Once you see the suggested drug names, you can click on a drug to replace the inline text with the selected drug name.
4. The drug definitions are located in the **Drugs Information Folder** inside the **Modules** folder.
5. The **Edit** functionality allows modification of the drug names in the descriptions.

---

## Conclusion

This project allows workers to manage and update patient records, assign patients to themselves, and integrate with an external drug system. The Admin Dashboard offers user role management and account CRUD operations, while the Worker Dashboard allows for patient record management, document handling, and integration with external systems.
