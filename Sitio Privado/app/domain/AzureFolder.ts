module app.domain {

    export interface IAzureFolder {
        Name: string;
        Blobs: app.domain.AzureBlob[];
    }

    export class AzureFolder extends app.domain.EntityBase implements IAzureFolder {
        constructor(public Name: string, public Blobs: app.domain.AzureBlob[]) {

            super();

            this.Name = Name;
            this.Blobs = Blobs;
        }
    }
}