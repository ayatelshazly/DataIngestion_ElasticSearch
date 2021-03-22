using DataIngestion.PublishAlbum.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace DataIngestion.PublishAlbum.Infrastructure.Services
{

    public class PublishAlbumService : IPublishAlbumService
    {
        public List<ArtistCollectionModel> artistCollectionModelList = new List<ArtistCollectionModel>();
        private readonly ILogger<PublishAlbumService> _logger;

        private List<ArtistModel> artistModelList = new List<ArtistModel>();
        private List<CollectionModel> collectionModelList = new List<CollectionModel>();
        private List<CollectionMatchModel> collectionMatchModelList = new List<CollectionMatchModel>();
        private List<AlbumModel> albumModelList = new List<AlbumModel>();


        public PublishAlbumService(ILogger<PublishAlbumService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #region Reading Files
        /// <summary>
        /// 1- First we need to Read All Files into list of relative models
        /// </summary>
        /// <param name="filePath"> passing Path that we will read Files From it </param>
        /// <returns></returns>
        public async Task<List<ArtistCollectionModel>> ReadAllFilesAsync(string filePath)
        {
            await ReadCollectionFileAsync(filePath);
            await ReadArtistFileAsync(filePath);
            await ReadCollectionMatchFileAsync(filePath);
            return await ReadArtistCollectionFileAsync(filePath);
        }
        public async Task<List<CollectionModel>> ReadCollectionFileAsync(string collectionFilePath)
        {
            collectionFilePath = collectionFilePath + "collection.txt";
            DateTime? releaseDate = null;
            bool? isCompilation = null;

            if (File.Exists(collectionFilePath))
            {
                string[] lines = await File.ReadAllLinesAsync(collectionFilePath);
                foreach (string line in lines)
                {
                    if (line.Contains("export_date") || line.Contains("dbTypes") || line.Contains("recordsWritten") || line.Contains("primaryKey"))
                        continue;
                    string[] listofstrings = ValidateLine(line); // return list of strings after removing unNeeded Charcters

                    ValidateNumberOfColumns(listofstrings, 18, line, "Collection "); // Check if Number of Columns of this table is Correct & throw Exception if columns count inCorrect

                    releaseDate = !string.IsNullOrEmpty(listofstrings[9]) ? DateTime.Parse(listofstrings[9]) : releaseDate;
                    isCompilation = !string.IsNullOrEmpty(listofstrings[16]) ? Convert.ToBoolean(Convert.ToInt16(listofstrings[16])) : isCompilation;

                    CollectionModel collectionModel = new CollectionModel(long.Parse(listofstrings[1]), listofstrings[2],
                        0, listofstrings[7], releaseDate, isCompilation, listofstrings[11], listofstrings[8]);
                    collectionModelList.Add(collectionModel);

                }
                return collectionModelList;
            }
            else
            {
                _logger.LogInformation($"Collection File Has A problem in reading..( Invalid Path )  ");
                throw new DirectoryNotFoundException();
            }

        }
        public async Task<List<ArtistModel>> ReadArtistFileAsync(string artistFilePath)
        {

            artistFilePath = artistFilePath + "artist.txt";
            if (File.Exists(artistFilePath))
            {
                string[] lines = await File.ReadAllLinesAsync(artistFilePath);
                foreach (string line in lines)
                {
                    if (line.Contains("export_date") || line.Contains("dbTypes") || line.Contains("recordsWritten") || line.Contains("primaryKey"))
                        continue;

                    string[] listofstrings = ValidateLine(line);  // return list of Lines after removing unNeeded Charcters
                    ValidateNumberOfColumns(listofstrings, 6, line, "Artist"); // Check if Number of Columns of this table is Correct & throw Exception if columns count inCorrect

                    listofstrings = line.Remove(line.IndexOf("\u0002")).Split("\u0001");
                    ArtistModel artistModel = new ArtistModel(int.Parse(listofstrings[1]), listofstrings[2]);
                    artistModelList.Add(artistModel);

                }
                return artistModelList;
            }
            else
            {
                _logger.LogInformation($"Artist File Has A problem in reading ..( Invalid Path )  ");
                throw new DirectoryNotFoundException();
            }

        }

        public async Task<List<CollectionMatchModel>> ReadCollectionMatchFileAsync(string collectionMatch)
        {
            long? upc = null;
            collectionMatch = collectionMatch + "collection_match.txt";

            if (File.Exists(collectionMatch))
            {
                string[] lines = await File.ReadAllLinesAsync(collectionMatch);
                foreach (string line in lines)
                {
                    if (line.Contains("export_date") || line.Contains("dbTypes") || line.Contains("recordsWritten") || line.Contains("primaryKey"))
                        continue;

                    string[] listofstrings = ValidateLine(line); // return list of Lines after removing unNeeded Charcters
                    ValidateNumberOfColumns(listofstrings, 5, line, "Collection Match"); // Check if Number of Columns of this table is Correct & throw Exception if columns count inCorrect

                    string numericUPC = !string.IsNullOrEmpty(listofstrings[2]) ? new String(listofstrings[2].Where(Char.IsDigit).ToArray()) : "";
                    upc = !string.IsNullOrEmpty(numericUPC) ? long.Parse(numericUPC) : upc;

                    CollectionMatchModel collectionMatchModel = new CollectionMatchModel(int.Parse(listofstrings[1]), upc);
                    collectionMatchModelList.Add(collectionMatchModel);

                }
                return collectionMatchModelList;
            }
            else
            {
                _logger.LogInformation($"Artist File Has A problem in reading..( Invalid Path ) ");
                throw new DirectoryNotFoundException();
            }


        }
        public async Task<List<ArtistCollectionModel>> ReadArtistCollectionFileAsync(string artistCollectionFilePath)
        {

            artistCollectionFilePath = artistCollectionFilePath + "artist_collection.txt";
            if (File.Exists(artistCollectionFilePath))
            {
                string[] lines = await File.ReadAllLinesAsync(artistCollectionFilePath);
                foreach (string line in lines)
                {
                    if (line.Contains("export_date") || line.Contains("dbTypes") || line.Contains("recordsWritten") || line.Contains("primaryKey"))
                        continue;
                    string[] listofstrings = ValidateLine(line);// return list of Lines after removing unNeeded Charcters
                    ValidateNumberOfColumns(listofstrings, 5, line, "Artist Collection"); // Check if Number of Columns of this table is Correct & throw Exception if columns count inCorrect
                    ArtistCollectionModel artistCollectionModel = new ArtistCollectionModel(int.Parse(listofstrings[1]), int.Parse(listofstrings[2]));
                    artistCollectionModelList.Add(artistCollectionModel);

                }
                return artistCollectionModelList;
            }
            else
            {
                _logger.LogInformation($"Artist_Collection File Has A problem in reading Invalid Path  ");
                throw new DirectoryNotFoundException();
            }

        }
        #endregion

        #region Feeding Album with Data
        /// <summary>
        /// Second We need to Feed All Models that we read before into AlbumList
        /// </summary>
        /// <param name="artistId"></param>
        /// <param name="collectionId"></param>
        /// <returns>AlbumModel</returns>
        public async Task<AlbumModel> FeedAlbumAsync(long artistId, long collectionId)
        {
            AlbumModel albumModel = new AlbumModel();

            albumModel = FeedAlbumWithCollectionData(collectionId, albumModel);
            FeedAlbumWithArtistsData(artistId, albumModel.Collection, albumModel);
            FeedAlbumWithCollectionMatchData(collectionId, albumModel);
            return albumModel;
        }

        private void FeedAlbumWithArtistsData(long artistId, CollectionModel collectionModel, AlbumModel albumModel)
        {
            try
            {
                if (collectionModel != null)
                {
                    var artistsIds = artistCollectionModelList.Where(e => e.CollectionId == collectionModel.Id).Select(e => e.AritistId).ToList();
                    List<ArtistModel> artistModels = artistModelList.Where(e => artistsIds.Contains(e.Id)).ToList();
                    if (artistModels != null && collectionModel != null)
                    {
                        albumModel.Artist = new List<ArtistModel>();
                        albumModel.Artist.AddRange(artistModels);
                        albumModelList.Add(albumModel);
                    }
                }

            }
            catch (Exception e)
            {
                _logger.LogInformation($"A problem in Feeding Album With ArtistModels" + collectionModel.Id);
            }
        }
        private AlbumModel FeedAlbumWithCollectionData(long collectionId, AlbumModel albumModel)
        {
            CollectionModel collectionModel;
            try
            {
                if (albumModelList.Where(e => e.Collection.Id == collectionId).Count() == 0)
                {
                    collectionModel = collectionModelList.FirstOrDefault(e => e.Id == collectionId);
                    albumModel.Collection = collectionModel;
                    return albumModel;
                }
                else
                {
                    albumModel = albumModelList.AsQueryable().FirstOrDefault(e => e.Collection.Id == collectionId);
                    return albumModel;
                }
            }
            catch (Exception e)
            {
                _logger.LogInformation($"A problem in Feeding Album With collection Model with collectionId :{0}" + collectionId);
                return albumModel;
            }
        }
        private AlbumModel FeedAlbumWithCollectionMatchData(long collectionId, AlbumModel albumModel)
        {
            CollectionMatchModel collectionMatchModel;
            try
            {
                collectionMatchModel = collectionMatchModelList.FirstOrDefault(e => e.CollectionId == collectionId);
                if (collectionMatchModel != null)
                {
                    albumModel.Collection.UPC = collectionMatchModel.UPC;
                }
                return albumModel;
            }
            catch (Exception e)
            {
                _logger.LogInformation($"A problem in Feeding Album With collectionMatch Model with collectionId :{0}" + collectionId);
                return albumModel;
            }
        }
        #endregion

        #region Validate Data
        private string[] ValidateLine(string line)
        {
            if (line.Contains("\u0002"))
                return line.Remove(line.IndexOf("\u0002")).Split("\u0001");
            else
                return line.Split("\u0001");
        }
        private void ValidateNumberOfColumns(string[] listofstrings, int columnsCount, string line, string fileType)
        {
            if (listofstrings.Count() != columnsCount)
            {
                _logger.LogInformation($"Can not Complete Reading {0} File, Make Sure that data is right at line :{1}", fileType, line);
                throw new ArgumentException();
            }
        }

    }
    #endregion

}
