module app.domain {
    export interface ISiteInformation {
        Id: number;
        UserId: number;
        AbbreviateName: string;
        SiteName: string;
        SiteType: string;
        Description: string;
        Url: string;
        CN: string;
        Priority: number;
    }

    export class SiteInformation extends app.domain.EntityBase implements ISiteInformation {
        constructor(public Id: number,
            public UserId: number,
            public AbbreviateName: string,
            public SiteName: string,
            public SiteType: string,
            public Description: string,
            public Url: string,
            public CN: string,
            public Priority: number) {

            super();
        }
    }
}