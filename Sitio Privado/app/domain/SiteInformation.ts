module app.domain {
    export interface ISiteInformation {
        id: number;
        userId: number;
        abbreviateName: string;
        siteName: string;
        siteType: string;
        description: string;
        url: string;
        cn: string;
        priority: number;
    }

    export class SiteInformation extends app.domain.EntityBase implements ISiteInformation {
        constructor(public id: number,
            public userId: number,
            public abbreviateName: string,
            public siteName: string,
            public siteType: string,
            public description: string,
            public url: string,
            public cn: string,
            public priority: number) {

            super();
        }
    }
}