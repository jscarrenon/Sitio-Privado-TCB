module app.domain {

    export interface IAzureBlob {
        Name: string;
        Url: string;
    }

    export class AzureBlob extends app.domain.EntityBase implements IAzureBlob {
        constructor(public Name: string, public Url: string) {

            super();

            this.Name = Name;
            this.Url = Url;
        }
    }
}