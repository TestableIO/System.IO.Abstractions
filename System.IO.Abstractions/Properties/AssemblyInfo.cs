﻿using System;
using System.Runtime.CompilerServices;

[assembly: CLSCompliant(true)]

#if DEBUG
    [assembly: InternalsVisibleTo("System.IO.Abstractions.TestingHelpers.Tests")]
#else
    [assembly: InternalsVisibleTo("System.IO.Abstractions.TestingHelpers.Tests, PublicKey=00240000048000009400000006020000002400005253413100040000010001001160c7a0f907c400c5392975b66d2f3752fb82625d5674d386b83896d4d4ae8d0ef8319ef391fbb3466de0058ad2f361b8f5cb8a32ecb4e908bece5c519387552cedd2ca0250e36b59c6d6dc3dc260ca73a7e27c3add4ae22d5abaa562225d7ba34d427e8f3f52928a46a674deb0208eca7d379aa22712355b91a55a5ce521d2")]
#endif

[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2,PublicKey=0024000004800000940000000602000000240000525341310004000001000100c547cac37abd99c8db225ef2f6c8a3602f3b3606cc9891605d02baa56104f4cfc0734aa39b93bf7852f7d9266654753cc297e7d2edfe0bac1cdcf9f717241550e0a7b191195b7667bb4f64bcb8e2121380fd1d9d46ad2d92d2d15605093924cceaf74c4861eff62abf69b9291ed0a340e113be11e6a7d3113e92484cf7045cc7")]