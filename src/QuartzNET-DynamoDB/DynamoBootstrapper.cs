﻿using System.Collections.Generic;
using System.Diagnostics;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

namespace Quartz.DynamoDB
{
    /// <summary>
    /// Bootstraps dynamo db to the required state. Ensures tables exist etc.
    /// </summary>
    public class DynamoBootstrapper
    {
        public void BootStrap(IAmazonDynamoDB client)
        {
            if (!TableExists(client, DynamoConfiguration.JobDetailTableName))
            {
                CreateJobDetailTable(client);
            }

            if (!TableExists(client, DynamoConfiguration.JobGroupTableName))
            {
                CreateJobGroupTable(client);
            }

            if (!TableExists(client, DynamoConfiguration.TriggerTableName))
            {
                CreateTriggerTable(client);
            }

            if (!TableExists(client, DynamoConfiguration.TriggerGroupTableName))
            {
                CreateTriggerGroupTable(client);
            }

            if (!TableExists(client, DynamoConfiguration.SchedulerTableName))
            {
                CreateSchedulerTable(client);
            }

            if (!TableExists(client, DynamoConfiguration.CalendarTableName))
            {
                CreateCalendarTable(client);
            }
        }

        private bool TableExists(IAmazonDynamoDB client, string tableName)
        {
            string lastEvaluatedTableName = null;

            do
            {
                // Create a request object to specify optional parameters.
                var request = new ListTablesRequest
                {
                    Limit = 10, // Page size.
                    ExclusiveStartTableName = lastEvaluatedTableName
                };

                var response = client.ListTables(request);

                if (response.TableNames.Contains(tableName))
                {
                    return true;
                }

                lastEvaluatedTableName = response.LastEvaluatedTableName;
            }
            while (lastEvaluatedTableName != null);

            return false;
        }

        private static void CreateJobDetailTable(IAmazonDynamoDB client)
        {
            // Build a 'CreateTableRequest' for the new table
            CreateTableRequest createRequest = new CreateTableRequest
            {
                TableName = DynamoConfiguration.JobDetailTableName,
                AttributeDefinitions = new List<AttributeDefinition>()
                {
                    new AttributeDefinition
                    {
                        AttributeName = "Group",
                        AttributeType = "S"
                    },
                    new AttributeDefinition
                    {
                        AttributeName = "Name",
                        AttributeType = "S"
                    }
                },
                KeySchema = new List<KeySchemaElement>()
                {
                    new KeySchemaElement
                    {
                        AttributeName = "Group",
                        KeyType = "HASH"
                    },
                    new KeySchemaElement
                    {
                        AttributeName = "Name",
                        KeyType = "RANGE"
                    }
                }
            };

            // Provisioned-throughput settings are required even though
            // the local test version of DynamoDB ignores them
            createRequest.ProvisionedThroughput = new ProvisionedThroughput(1, 1);

            // Using the DynamoDB client, make a synchronous CreateTable request
            CreateTableResponse createResponse;
            createResponse = client.CreateTable(createRequest);

            // Report the status of the new table...
            Debug.WriteLine("\n\n Created the \"Job\" table successfully!\n    Status of the new table: '{0}'",
                createResponse.TableDescription.TableStatus);
        }

        private void CreateJobGroupTable(IAmazonDynamoDB client)
        {
            // Build a 'CreateTableRequest' for the new table
            CreateTableRequest createRequest = new CreateTableRequest
            {
                TableName = DynamoConfiguration.JobGroupTableName,
                AttributeDefinitions = new List<AttributeDefinition>()
                {
                    new AttributeDefinition
                    {
                        AttributeName = "Name",
                        AttributeType = "S"
                    }
                },
                KeySchema = new List<KeySchemaElement>()
                {
                    new KeySchemaElement
                    {
                        AttributeName = "Name",
                        KeyType = "HASH"
                    }
                }
            };

            // Provisioned-throughput settings are required even though
            // the local test version of DynamoDB ignores them
            createRequest.ProvisionedThroughput = new ProvisionedThroughput(1, 1);

            // Using the DynamoDB client, make a synchronous CreateTable request
            CreateTableResponse createResponse;
            createResponse = client.CreateTable(createRequest);

            // Report the status of the new table...
            Debug.WriteLine("\n\n Created the \"Job Group\" table successfully!\n    Status of the new table: '{0}'",
                createResponse.TableDescription.TableStatus);
        }

        private void CreateCalendarTable(IAmazonDynamoDB client)
        {
            // Build a 'CreateTableRequest' for the new table
            CreateTableRequest createRequest = new CreateTableRequest
            {
                TableName = DynamoConfiguration.CalendarTableName,
                AttributeDefinitions = new List<AttributeDefinition>()
                {
                    new AttributeDefinition
                    {
                        AttributeName = "Name",
                        AttributeType = "S"
                    }
                },
                KeySchema = new List<KeySchemaElement>()
                {
                    new KeySchemaElement
                    {
                        AttributeName = "Name",
                        KeyType = "HASH"
                    }
                }
            };

            // Provisioned-throughput settings are required even though
            // the local test version of DynamoDB ignores them
            createRequest.ProvisionedThroughput = new ProvisionedThroughput(1, 1);

            // Using the DynamoDB client, make a synchronous CreateTable request
            CreateTableResponse createResponse;
            createResponse = client.CreateTable(createRequest);

            // Report the status of the new table...
            Debug.WriteLine("\n\n Created the \"Calendar\" table successfully!\n    Status of the new table: '{0}'",
                createResponse.TableDescription.TableStatus);
        }

        private void CreateTriggerGroupTable(IAmazonDynamoDB client)
        {
            // Build a 'CreateTableRequest' for the new table
            CreateTableRequest createRequest = new CreateTableRequest
            {
                TableName = DynamoConfiguration.TriggerGroupTableName,
                AttributeDefinitions = new List<AttributeDefinition>()
                {
                    new AttributeDefinition
                    {
                        AttributeName = "Name",
                        AttributeType = "S"
                    }
                },
                KeySchema = new List<KeySchemaElement>()
                {
                    new KeySchemaElement
                    {
                        AttributeName = "Name",
                        KeyType = "HASH"
                    }
                }
            };

            // Provisioned-throughput settings are required even though
            // the local test version of DynamoDB ignores them
            createRequest.ProvisionedThroughput = new ProvisionedThroughput(1, 1);

            // Using the DynamoDB client, make a synchronous CreateTable request
            CreateTableResponse createResponse;
            createResponse = client.CreateTable(createRequest);

            // Report the status of the new table...
            Debug.WriteLine("\n\n Created the \"Trigger Group\" table successfully!\n    Status of the new table: '{0}'",
                createResponse.TableDescription.TableStatus);
        }

        private void CreateTriggerTable(IAmazonDynamoDB client)
        {
            // Build a 'CreateTableRequest' for the new table
            CreateTableRequest createRequest = new CreateTableRequest
            {
                TableName = DynamoConfiguration.TriggerTableName,
                AttributeDefinitions = new List<AttributeDefinition>()
                {
                    new AttributeDefinition
                    {
                        AttributeName = "Group",
                        AttributeType = "S"
                    },
                    new AttributeDefinition
                    {
                        AttributeName = "Name",
                        AttributeType = "S"
                    }
                },
                KeySchema = new List<KeySchemaElement>()
                {
                    new KeySchemaElement
                    {
                        AttributeName = "Group",
                        KeyType = "HASH"
                    },
                    new KeySchemaElement
                    {
                        AttributeName = "Name",
                        KeyType = "RANGE"
                    }
                }
            };

            // Provisioned-throughput settings are required even though
            // the local test version of DynamoDB ignores them
            createRequest.ProvisionedThroughput = new ProvisionedThroughput(1, 1);

            // Using the DynamoDB client, make a synchronous CreateTable request
            CreateTableResponse createResponse;
            createResponse = client.CreateTable(createRequest);

            // Report the status of the new table...
            Debug.WriteLine("\n\n Created the \"Trigger\" table successfully!\n    Status of the new table: '{0}'",
                createResponse.TableDescription.TableStatus);
        }

        private void CreateSchedulerTable(IAmazonDynamoDB client)
        {
            // Build a 'CreateTableRequest' for the new table
            CreateTableRequest createRequest = new CreateTableRequest
            {
                TableName = DynamoConfiguration.SchedulerTableName,
                AttributeDefinitions = new List<AttributeDefinition>()
                {
                    new AttributeDefinition
                    {
                        AttributeName = "InstanceId",
                        AttributeType = "S"
                    }
                },
                KeySchema = new List<KeySchemaElement>()
                {
                    new KeySchemaElement
                    {
                        AttributeName = "InstanceId",
                        KeyType = "HASH"
                    }
                }
            };

            // Provisioned-throughput settings are required even though
            // the local test version of DynamoDB ignores them
            createRequest.ProvisionedThroughput = new ProvisionedThroughput(1, 1);

            // Using the DynamoDB client, make a synchronous CreateTable request
            CreateTableResponse createResponse;
            createResponse = client.CreateTable(createRequest);

            // Report the status of the new table...
            Debug.WriteLine("\n\n Created the \"Scheduler\" table successfully!\n    Status of the new table: '{0}'",
                createResponse.TableDescription.TableStatus);
        }

    }
}