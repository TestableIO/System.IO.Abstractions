using System.Text;

namespace System.IO.Abstractions.TestingHelpers
{

    public class MockStreamWriter : StreamWriter
    {
        readonly IMockFileDataAccessor mockFileDataAccessor;
        readonly string path;
        public MockStreamWriter(Stream stream) : base(stream)
        {
            
        }

        public MockStreamWriter(string path, IMockFileDataAccessor mockFileDataAccessor) :base(mockFileDataAccessor.FileInfo.FromFileName(path).OpenWrite())
        {
           
            this.mockFileDataAccessor = mockFileDataAccessor;
            this.path = path;
            

            if (mockFileDataAccessor.FileExists(path))
            {
                /* only way to make an expandable MemoryStream that starts with a particular content */
                var data = mockFileDataAccessor.GetFile(path).TextContents;
                base.Write(data);
                
            }
            
        }

        public override void Write(string value)
        {
            //Inspect the value and do something
            base.Write(value);

            
        }

        public override void WriteLine(string value)
        {
            //Inspect the value and do something
            base.WriteLine(value);


        }
   

       
    }

   
}