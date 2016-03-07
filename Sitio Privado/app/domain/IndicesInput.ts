module app.domain {
    export interface IIndicesInput {
        xfecha: string;
    }

    export class IndicesInput extends app.domain.InputBase implements IIndicesInput {
        constructor(public xfecha: string) {

            super();
        }
    }
}