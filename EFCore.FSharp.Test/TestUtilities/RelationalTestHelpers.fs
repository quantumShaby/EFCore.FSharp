﻿namespace Bricelam.EntityFrameworkCore.FSharp.Test.TestUtilities

open Microsoft.EntityFrameworkCore.TestUtilities
open Microsoft.EntityFrameworkCore.Infrastructure
open Microsoft.Extensions.DependencyInjection
open Bricelam.EntityFrameworkCore.FSharp.Test.TestUtilities.FakeProvider

type RelationalTestHelpers private () =
    inherit TestHelpers()

    static let instance = RelationalTestHelpers()
    static member Instance = instance
    member this.Action() = printfn "action"

    override this.AddProviderServices services =
        FakeRelationalOptionsExtension.AddEntityFrameworkRelationalDatabase services

    override this.UseProviderOptions optionsBuilder =

        let e = optionsBuilder.Options.FindExtension<FakeRelationalOptionsExtension>()
        let extension =
            match isNull e with
            | true -> FakeRelationalOptionsExtension()
            | false -> e


        let fakeConn = new FakeDbConnection("Database=Fake")

        (optionsBuilder :> IDbContextOptionsBuilderInfrastructure).AddOrUpdateExtension(extension.WithConnection(fakeConn))