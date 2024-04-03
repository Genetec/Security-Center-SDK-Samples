// ==========================================================================
// Copyright (C) 2021 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using System;
using Genetec.Sdk.Workspace.Components.CardholderFieldsExtractor;

namespace CardholderFieldsExtractor.Extractors
{
    /// <summary>
    /// Cardholder fields extractor sample using Test Application ABC certificate
    /// </summary>
    public sealed class
        GenetecCardholderFieldsExtractor : Genetec.Sdk.Workspace.Components.CardholderFieldsExtractor.
            CardholderFieldsExtractor
    {
        private readonly Lazy<Guid> m_uniqueLazyId =
            new Lazy<Guid>(() => new Guid("{63c51eb6-2ebb-4229-a016-c4e190aaec74}"));

        public override string Name => "Genetec CardholderFieldsExtractor";

        public override Guid UniqueId => m_uniqueLazyId.Value;

        /**
         * Populate cardholder fields with extracted data
         */
        public override CardholderFields GetFields(CardholderFieldsExtractorData data)
        {
            var results = new CardholderFields
            {
                Description = data.Cardholder.ToString(),
                Email = "test@test.com",
                FirstName = "test",
                LastName = "test",
            };


            return results;
        }
    }
}