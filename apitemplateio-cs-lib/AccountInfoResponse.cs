using System;

namespace apitemplateio
{

    public class AccountInfoResponse    {
        public string status { get; set; }
        public string subscription_product { get; set; }
        public DateTime subscription_current_period_start { get; set; }
        public DateTime subscription_current_period_end { get; set; }
        public string subscription_status { get; set; }
        public string subscription_interval { get; set; }
        public int api_quota { get; set; }
        public int api_remaining { get; set; }
        public int api_used { get; set; }
        public int template_remaining { get; set; }
        public int template_count { get; set; }
        public int template_quota { get; set; }
        public string message { get; set; }
    }


}
