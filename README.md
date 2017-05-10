# Azure Storage Integration with CDN, Search and HDInsight

In this sample, we demonstrate how to host, analyze and search content uploaded to Azure Storage blobs. This code was originally used in the [2017 Azure Storage Build Talk]().

If you don't have a Microsoft Azure subscription you can get a FREE trial account [here](https://azure.microsoft.com/free/?WT.mc_id=A7833027B).

## Running this sample

To run this sample:

1. Create an [Azure Storage Account](https://docs.microsoft.com/azure/storage/storage-create-storage-account). Select general purpose, not blob storage, for the account type.

2. Unzip 'clinical-trials.zip'. It contains a 100 file subsample of the full clinical trials data set which you can download at [clinicaltrials.gov](https://clinicaltrials.gov/ct2/results?show_down=Y).

3. Create a container named 'data' in your storage account and upload the unzipped files to a virtual directory named 'clinical-trials'. You can do this using [Azure CLI 2.0 with Azure Storage](https://docs.microsoft.com/azure/storage/storage-azure-cli#create-and-manage-blobs). Reference [az storage blob upload-batch](https://docs.microsoft.com/cli/azure/storage/blob#upload-batch) for detailed guidance. You can also take advantage of [AzCopy command-line utility](https://docs.microsoft.com/azure/storage/storage-use-azcopy), Azure Storage client libraries, or other [Azure Storage client tools](https://docs.microsoft.com/azure/storage/storage-explorers).

4. Make sure to [Grant anonymous users permission](https://docs.microsoft.com/azure/storage/storage-manage-access-to-resources#grant-anonymous-users-permissions-to-containers-and-blobs) to the storage container.

5. [Use the Azure CDN to access blobs with custom domains over HTTPS](https://docs.microsoft.com/en-us/azure/storage/storage-https-custom-domain-cdn). When adding your CDN endpoint make sure to enter '/data' in the 'origin path' field. The clinical trials will be accessible at '&lt;domain-name&gt;/clinical-trials/&lt;file-name&gt;'.

6. To perform a full text search on the clinical trials data, [Integrate Azure Search with blob storage](https://docs.microsoft.com/en-us/azure/search/search-blob-storage-integration) directly from the [Azure portal](https://portal.azure.com/). When creating your data source, select the 'data' container from the storage account you created. Input 'clinical-trials' in the 'blob folder' field. When constructing your index, make sure the 'content' field is 'searchable'. You can immediately start to [Query your Azure Search index using the Azure Portal](https://docs.microsoft.com/azure/search/search-explorer), even before all of the documents have been indexed.

7. To convert the clinical trials text files to JSON, [Create an Apache Spark cluster in Azure HDInsight](https://docs.microsoft.com/azure/hdinsight/hdinsight-apache-spark-jupyter-spark-sql). When creating your cluster, make sure to select the storage account you created earlier and specify 'data' as the default container. Open Jupyter and create a PySpark notebook. Instructions can be found at the previous linked article. Finally, run the code in 'texttojson.py'.

8. To search the newly generated JSON files, execute the REST requests in 'jsonsearchsetup.txt'. You'll need to fill in the key and name of both your search service and storage account. To better understand the requests in 'jsonsearchsetup.txt', see [Indexing JSON blobs with Azure Search blob indexer](https://docs.microsoft.com/en-us/azure/search/search-howto-index-json-blobs).

9. To create a front end for your search service, visit the [Azure Search Generator](http://azsearchstore.azurewebsites.net/azsearchgenerator/index.html). Input the query key for your search service, name of your search service, and the JSON schema for your index. Executing the second request in 'jsonsearchsetup.txt' to create your index will return the JSON schema. Instead of using the Azure Search Generator, you can also fill in your search service query key and name in 'search-clinical-trials.htm'.

10. To access your front end, enable CORS on your search index directly from the [Azure portal](https://portal.azure.com/), and upload the .HTM file to the 'data' container in your storage account. You can then access it via the custom domain you set up in step 5.

## Appendix

A very brief example showing you how to do a cross tabulation on the clinical trials data in PySpark can be found in quickstats.py. Run the code in the PySpark notebook you set up in step 7.

You can search just the metadata of your storage blobs by selecting 'storage metadata only' in the 'data to extract' field when creating a data source for your search index (as in step 6). The query in 'metadatasearchquery.txt' returns the 3 largest blobs greater than 195 GB in descending size order.
