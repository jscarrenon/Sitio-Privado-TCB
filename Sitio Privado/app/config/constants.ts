module app.config {
    // Kunder development and testing servers
    export const CONFIG = {
        ENV: 'Local',
        CLIENT_ID: 'passwordgrant',
        CLIENT_SECRET: 'secret',
        OAUTH2_URL: 'http://localhost:51928/',
        TANNER_PUBLIC_SITE_URL: 'http://www.kunder.cl/tannercl/'
    };
}
