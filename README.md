# Multi-Account Push Service API Receiver Example Application
This is an example API application, in C# on .NET Framework, showing the implementation of a MAPS Transaction Receiver Endpoint.

You can clone this repository, and implement your own logic in the MapsTransactionController class, in the ReceiveTransaction method.

## Pre-defined Configuration

An example API key and the Debitech Public IP address has been included in the web.config file. It is important that you replace the API key in this config file, with the key provided by Debitech on initial setup.

The NLog.config file in the root directory can be used to control the output of the logging. 
More information can be found on the NLog website: https://nlog-project.org/config/
