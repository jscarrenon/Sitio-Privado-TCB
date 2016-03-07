module app.domain {

    export interface IAzureFolder {
        Name: string;
        Blobs: app.domain.AzureBlob[];
        Folders: app.domain.AzureFolder[];
    }

    export class AzureFolder extends app.domain.EntityBase implements IAzureFolder {
        constructor(public Name: string, public Blobs: app.domain.AzureBlob[], public Folders: app.domain.AzureFolder[]) {

            super();
        }
    }
}