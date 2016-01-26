module App.Domain {

    export interface IAzureFolder {
        Name: string;
        Blobs: App.Domain.AzureBlob[];
    }

    export class AzureFolder extends App.Domain.EntityBase implements IAzureFolder {
        constructor(public Name: string, public Blobs: App.Domain.AzureBlob[]) {

            super();

            this.Name = Name;
            this.Blobs = Blobs;
        }
    }
}