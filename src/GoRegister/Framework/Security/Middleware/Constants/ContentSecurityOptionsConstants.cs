using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoRegister.Framework.Security.Middleware.Constants
{
    public static class ContentSecurityOptionsConstants
    {
        public static readonly string Header = "Content-Security-Policy";
        // public static readonly string Value = "base-uri 'self'; block-all-mixed-content; default-src 'self'; img-src 'self' data: https: https://goregister-development.s3.eu-west-1.amazonaws.com/; object-src 'none';  script-src 'self' 'unsafe-inline' 'unsafe-eval' ; style-src 'self' https://fonts.googleapis.com 'unsafe-inline'; font-src 'self' https://fonts.gstatic.com;  upgrade-insecure-requests; ";
        //public static readonly string Value = "base-uri 'self'; block-all-mixed-content; default-src 'self' https://meetingscentral-staging.amexgbt.com https://meetingscentral-demo.amexgbt.com https://meetingscentral.amexgbt.com; img-src 'self' data: https: https://gbt-goregister-mrf-v19-publics3bucket-5q3dpsgtp101.s3.eu-west-1.amazonaws.com/; object-src 'none';  script-src 'self' 'unsafe-inline' 'unsafe-eval' https://cdnjs.cloudflare.com; style-src 'self' https://fonts.googleapis.com 'unsafe-inline'; font-src 'self' https://fonts.gstatic.com;  upgrade-insecure-requests; ";

        public static readonly string Value = "base-uri 'self'; block-all-mixed-content; default-src 'self' https://meetingscentral-staging.amexgbt.com https://meetingscentral-demo.amexgbt.com https://meetingscentral-beta.amexgbt.com https://meetingscentral.amexgbt.com; img-src 'self' data: https: https://gbt-goregister-mrf-beta-v1-publics3bucket-qce0ijsbd6i9.s3.eu-west-1.amazonaws.com/; object-src 'none';  script-src 'self' 'unsafe-inline' 'unsafe-eval' https://cdnjs.cloudflare.com; style-src 'self' https://fonts.googleapis.com 'unsafe-inline'; font-src 'self' https://fonts.gstatic.com;  upgrade-insecure-requests; ";
    }
}
