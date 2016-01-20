module app.domain {

    export interface IAzureContainer {
        Name: string;
    }

    export class AzureContainer extends app.domain.EntityBase implements IAzureContainer {
        constructor(public Name: string) {

            super();

            this.Name = Name;
        }
    }
}