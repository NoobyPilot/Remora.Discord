//
//  GuildUpdateTests.cs
//
//  Author:
//       Jarl Gullberg <jarl.gullberg@gmail.com>
//
//  Copyright (c) Jarl Gullberg
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
//
//  You should have received a copy of the GNU Lesser General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//

using System.Text.Json;
using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.API.Tests.TestBases;
using Remora.Rest.Xunit;

namespace Remora.Discord.API.Tests.Gateway.Events;

/// <summary>
/// Tests the Hello event.
/// </summary>
public class GuildUpdateTests : GatewayEventTestBase<IGuildUpdate>
{
    /// <inheritdoc />
    protected override JsonAssertOptions AssertOptions { get; } = JsonAssertOptions.Default with
    {
        AllowMissing = new[]
        {
            "hoisted_role", // internal discord value
            "guild_hashes", // internal discord value
            "lazy", // undocumented value
            "nsfw", // undocumented value, presumably duplicate of "nsfw_level"
            "region", // deprecated value
            "guild_id" // undocumented value
        },

        AllowSkip = e => e.ValueKind is JsonValueKind.String && e.GetString() == "REMORA_UNKNOWN_FEATURE"
    };
}
