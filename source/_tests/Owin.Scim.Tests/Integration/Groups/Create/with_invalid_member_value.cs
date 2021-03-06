﻿namespace Owin.Scim.Tests.Integration.Groups.Create
{
    using System.Net;
    using Machine.Specifications;
    using Model.Groups;

    using v2.Model;

    public class with_invalid_member_value : when_creating_a_group
    {
        Establish context = () =>
        {
            GroupDto = new ScimGroup2
            {
                DisplayName = "hello",
                ExternalId = "hello",
                Members = new []
                {
                    new Member {Value = "bogus value", Type = "user" },
                }
            };
        };

        It should_return_bad_request = () => Response.StatusCode.ShouldEqual(HttpStatusCode.BadRequest);

        It should_return_invalid_syntax = () => Error.ScimType.ShouldEqual(Model.ScimErrorType.InvalidSyntax);
    }
}