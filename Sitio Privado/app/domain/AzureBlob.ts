module app.domain {

    export interface IAzureBlob {
        Name: string;
        Link: string;
    }

    export class AzureBlob extends app.domain.EntityBase implements IAzureBlob {
        constructor(public Name: string, public Link: string) {

            super();

            this.Name = Name;
            this.Link = Link;
        }
    }
}