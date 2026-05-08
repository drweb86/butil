# Azure Blob Storage Setup

BUtil supports **Azure Blob Storage** using Shared Key authentication (account name + account key). Backups are stored as blobs inside a container of your choice.

## Fields

| Field | Description |
|---|---|
| **Account Name** | Your storage account name (e.g. `mystorageaccount`). |
| **Account Key** | One of the two 512-bit access keys for the storage account. |
| **Container** | The blob container name. Create it in advance in the Azure portal. |
| **Path Prefix** | Optional prefix inside the container (e.g. `backups/laptop`). Useful when sharing one container across multiple machines. |

---

## Setup

1. In the [Azure Portal](https://portal.azure.com/) open or create a **Storage Account**.
   - Recommended settings: **Locally-redundant storage (LRS)** for cost-effectiveness, **Cool** access tier for infrequent backup access.
2. Inside the storage account, go to **Containers** and create a new container (e.g. `butil-backups`). Set access level to **Private**.
3. Go to **Security + networking → Access keys** in the storage account.
4. Copy the **Storage account name** and either **Key 1** or **Key 2**.
5. In BUtil:
   - **Account Name**: your storage account name
   - **Account Key**: the key copied in step 4
   - **Container**: the container name from step 2
   - **Path Prefix**: optional; leave empty to store at the container root

---

## Permissions

Shared Key gives full access to the storage account. For a more restricted setup, you can instead use a **SAS token** — however BUtil currently requires Shared Key authentication. To limit access at the network level, configure **Firewall and virtual networks** in the storage account settings.

---

## Troubleshooting

- **AuthenticationFailed** — the account name or key is incorrect. Verify by copying them again from the Azure portal.
- **ContainerNotFound** — the container name is misspelled or the container does not exist. Create it manually in the portal first.
- **The specified resource does not exist** — the blob or path does not exist yet; this is expected on the first backup run.
- **Connection timeout** — verify network connectivity and that no firewall is blocking outbound HTTPS to `*.blob.core.windows.net`.
