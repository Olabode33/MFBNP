﻿using Abp.Runtime.Validation;
using PMSDemo.Dto;

namespace PMSDemo.MultiTenancy.Payments.Dto
{
    public class GetPaymentHistoryInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "CreationTime";
            }

            Sorting = Sorting.Replace("editionDisplayName", "Edition.DisplayName");
        }
    }
}
