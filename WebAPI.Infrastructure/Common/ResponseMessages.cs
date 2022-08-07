using System;
using System.Collections.Generic;
using System.Text;

namespace WebAPI.Infrastructure.Common
{
    public enum ResponseMessages
    {
        [StringValue("No record found")]
        RecordNotFound = 101,
        [StringValue("File already exist, Please try with the another file or rename the file.")]
        FileAlreadyExists = 102,
        [StringValue("Requested content does not exists.")]
        DoesNotExists = 103,
        [StringValue("Please choose a file and then try to upload.")]
        PleaseSelectAFile = 104,
        [StringValue("Request Executed Successfully.")]
        RequestExecutedSuccessfully = 201,
        [StringValue("Bad request")]
        BadRquest = 404

    }
}
