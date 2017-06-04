module app.config {
    // Kunder development and testing servers
    export const CONFIG = {
        ENV: 'qa',
        /* CLIENT_ID: 'dev_customer_password_4',
        CLIENT_SECRET: 'rdkbKVM7neKs15wX0YgOzQjTZ8JF4SRNYs-KAjNr',
        OAUTH2_URL: 'https://oauthdesa.tanner.cl/',
        TANNER_PUBLIC_SITE_URL: 'http://www.kunder.cl/tannercl/',
        TANNER_AUTHENTICATION_API: 'https://proxylogindesa.azurewebsites.net/api/authentication/',
        REQUIRED_GROUP: 'clientescorredora' */
		CLIENT_ID: 'dev_customer_password_4',
		CLIENT_SECRET: 'TsmoyGPJzsplYbZ_XuyGwqd5KfHmU_QA6cf8u31U',
		/*CLIENT_ID: 'dev_customer_password',
		CLIENT_SECRET: 'rdkbKVM7neKs15wX0YgOzQjTZ8JF4SRNYs-KAjNr',*/
		OAUTH2_URL: 'https://oauthqa.tanner.cl/',
		TANNER_PUBLIC_SITE_URL: 'http://sitiopublicoqa.tanner.cl/',
		TANNER_AUTHENTICATION_API: 'https://proxyloginqa.azurewebsites.net/api/authentication/',
		REQUIRED_GROUP: 'ClientesCorredora'
    };
}
