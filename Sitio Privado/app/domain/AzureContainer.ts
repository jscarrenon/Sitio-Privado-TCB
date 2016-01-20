module app.domain {

    export interface IAzureContainer {
        Name: string;
        Blobs: app.domain.AzureBlob[];
    }

    export class AzureContainer extends app.domain.EntityBase implements IAzureContainer {
        constructor(public Name: string, public Blobs: app.domain.AzureBlob[]) {

            super();

            this.Name = Name;
            this.Blobs = Blobs;
        }
    }
}