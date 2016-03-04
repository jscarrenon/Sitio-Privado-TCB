module app.domain {
    export interface IAgenteInput {
    }

    export class AgenteInput extends app.domain.InputBase implements IAgenteInput {
        constructor() {

            super();
        }
    }
}