module app.config {
    // Kunder development and testing servers
    export const CONFIG = {
        ENV: 'debug',
        CLIENT_ID: 'passwordgrant',
        CLIENT_SECRET: 'secret',
        OAUTH2_URL: 'https://oauthdesa.tanner.cl/',
        TANNER_PUBLIC_SITE_URL: 'http://www.kunder.cl/tannercl2/',
        TANNER_AUTHENTICATION_API: 'http://localhost:63800/api/authentication/'
    };
}
