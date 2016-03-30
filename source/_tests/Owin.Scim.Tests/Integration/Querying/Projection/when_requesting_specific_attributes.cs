﻿namespace Owin.Scim.Tests.Integration.Querying.Projection
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;

    using Extensions;

    using Machine.Specifications;

    using Model.Users;

    using Ploeh.AutoFixture;

    using Users;

    public class when_requesting_specific_attributes : using_a_scim_server
    {
        Because of = async () =>
        {
            var autoFixture = new Fixture();

            var existingUser = autoFixture.Build<User>()
                .With(x => x.UserName, UserNameUtility.GenerateUserName())
                .With(x => x.Password, "somePass")
                .With(x => x.PreferredLanguage, "en-US,en,es")
                .With(x => x.Locale, "en-US")
                .With(x => x.Timezone, @"US/Eastern")
                .With(x => x.Emails, null)
                .With(x => x.PhoneNumbers, null)
                .With(x => x.Ims, null)
                .With(x => x.Photos, null)
                .With(x => x.Addresses, null)
                .Create(seed: new User());

            // Insert the first user so there's one already in-memory.
            await (await Server
                .HttpClient
                .PostAsync(new UriBuilder(new Uri("http://localhost/users")) { Query = "attributes=" + string.Join(",", Attributes ?? new List<string>()) }.ToString(), new ObjectContent<User>(existingUser, new ScimJsonMediaTypeFormatter()))
                .AwaitResponse()
                .AsTask).DeserializeTo(() => JsonResponse);
        };

        protected static IList<string> Attributes;

        protected static IDictionary<string, object> JsonResponse;
    }
}