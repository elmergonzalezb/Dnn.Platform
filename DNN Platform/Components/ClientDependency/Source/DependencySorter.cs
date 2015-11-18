﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClientDependency.Core
{
    /// <summary>
    /// Utility for sorting dependencies based on their priority and the index in which they exist in the list (# they were added)
    /// </summary>
    internal static class DependencySorter
    {
        /// <summary>
        /// Sort the items by their priority and their index they currently exist in the collection
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public static IList<IClientDependencyFile> SortItems(IList<IClientDependencyFile> files)
        {
            //first check if each item's order is the same, if this is the case we'll make sure that we order them 
            //by the way they were defined
            if (!files.Any()) return files;

            // check if we need to manage frameworks
            if (files.Any(f => f.Framework != ""))
            {
                var newList = files.Where(f => f.Framework == "").ToList();
                var frameworks = files.Where(f => f.Framework != "").GroupBy(f => f.Framework);
                foreach (var framework in frameworks)
                {
                    var topPriority = framework.FirstOrDefault(f => f.RemoveFramework);
                    if (topPriority != null)
                    {
                        if (topPriority.FilePath != "")
                        {
                            newList.Add(topPriority);
                        }
                    }
                    else
                    {
                        newList.Add(framework.OrderByDescending(f => f.Version).First());
                    }
                }
                files = newList;
            }

            var firstPriority = files.First().Priority;

            if (files.Any(x => x.Priority != firstPriority))
            {
                var sortedOutput = new List<IClientDependencyFile>();
                //ok they are not the same so we'll need to sort them by priority and by how they've been entered
                var groups = files.GroupBy(x => x.Priority).OrderBy(x => x.Key);
                foreach (var currentPriority in groups)
                {
                    //for this priority group, we'll need to prioritize them by how they are found in the files array
                    sortedOutput.AddRange(currentPriority.OrderBy(files.IndexOf));
                }
                return sortedOutput;
            }

            //they are all the same so we can really just return the original list since it will already be in the 
            //order that they were added.
            return files;
        }

    }
}
