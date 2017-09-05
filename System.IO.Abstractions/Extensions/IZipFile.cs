namespace Sitecore.Diagnostics.IO
{
  using System;
  using JetBrains.Annotations;

  public interface IZipFile : IDisposable
  {
    IFile File { get; }

    void ExtractTo([NotNull] IFolder folder);

    IZipFileEntries Entries { get; }
  }
}