module app.domain {
    export interface IChangePasswordInput {
        OldPassword: string;
        Password: string;
        PasswordValidation: string;
    }

    export class ChangePasswordInput extends app.domain.InputBase implements IChangePasswordInput {
        constructor(public OldPassword?: string,
            public Password?: string,
            public PasswordValidation?: string) {

            super();
        }
    }
}