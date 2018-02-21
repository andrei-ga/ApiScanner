export interface AccountModel {
    id?: string,
    email?: string,
    password?: string,
    passwordRepeat?: string,
    rememberLogin?: boolean,
    subscribe?: boolean,
    resetToken?: string,
    windowsLogin?: boolean,
}