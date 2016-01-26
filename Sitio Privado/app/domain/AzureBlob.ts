module App.Domain {

    export interface IAzureBlob {
        Name: string;
        Url: string;
    }

    export class AzureBlob extends App.Domain.EntityBase implements IAzureBlob {
        constructor(public Name: string, public Url: string) {

            super();

            this.Name = Name;
            this.Url = Url;
        }
    }
}