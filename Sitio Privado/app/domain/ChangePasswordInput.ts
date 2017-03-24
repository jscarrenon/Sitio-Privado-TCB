module app.domain {
    export interface IChangePasswordInput {
        OldPassword: string;
        NewPassword: string;
        PasswordValidation: string;
    }

    export class ChangePasswordInput extends app.domain.InputBase implements IChangePasswordInput {
        constructor(public OldPassword?: string,
            public NewPassword?: string,
            public PasswordValidation?: string) {

            super();
        }
    }
}