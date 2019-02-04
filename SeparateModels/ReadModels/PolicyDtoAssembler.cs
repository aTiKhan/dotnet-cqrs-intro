using System.Collections.Generic;
using System.Linq;
using SeparateModels.Domain;

namespace SeparateModels.ReadModels
{
    public static class PolicyVersionDtoAssembler
    {
        public static PolicyVersionDto AssemblePolicyVersionDto(Policy policy, PolicyVersion version)
        {
            return new PolicyVersionDto
            {
                Id = version.Id,
                PolicyId = policy.Id,
                PolicyNumber = policy.Number,
                ProductCode = policy.ProductCode,
                VersionNumber = version.VersionNumber,
                VersionStatus = version.VersionStatus.ToString(),
                PolicyStatus = version.PolicyStatus.ToString(),
                PolicyHolder = $"{version.PolicyHolder.LastName} {version.PolicyHolder.FirstName}",
                Insured = null,
                Car = $"{version.Car.PlateNumber} {version.Car.Make} {version.Car.ProductionYear}",
                CoverFrom = version.CoverPeriod.ValidFrom,
                CoverTo = version.CoverPeriod.ValidTo,
                VersionFrom = version.VersionValidityPeriod.ValidFrom,
                VersionTo = version.VersionValidityPeriod.ValidTo,
                TotalPremium = version.TotalPremium.Amount,
                Covers = version.Covers.Select(CoverDtoAssembler.AssembleCoverDto).ToList(),
                Changes = AssembleChanges(policy, version)
            };
        }

        private static List<string> AssembleChanges(Policy policy, PolicyVersion version)
        {
            if (!version.BaseVersionNumber.HasValue)
                return new List<string>() {};

            var baseVersion = policy.Versions.WithNumber(version.BaseVersionNumber.Value);

            return PolicyVersionComparer.Of(version, baseVersion).Compare();
        }
    }

    public static class CoverDtoAssembler
    {
        public static CoverDto AssembleCoverDto(PolicyCover policyCover)
        {
            return new CoverDto
            {
                Id = policyCover.Id,
                Code = policyCover.CoverCode,
                CoverFrom = policyCover.CoverPeriod.ValidFrom,
                CoverTo = policyCover.CoverPeriod.ValidTo,
                PremiumAmount = policyCover.Amount.Amount
            };
        }
    }
}