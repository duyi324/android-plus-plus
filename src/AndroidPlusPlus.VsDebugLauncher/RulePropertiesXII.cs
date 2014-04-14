﻿////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;

using Microsoft.VisualStudio.ProjectSystem;
using Microsoft.VisualStudio.ProjectSystem.Utilities;
using Microsoft.VisualStudio.ProjectSystem.Properties;

using AndroidPlusPlus.Common;

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

namespace AndroidPlusPlus.VsDebugLauncher.Rules
{

  ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
  ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
  ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

  [Export]

  [PartMetadata(ProjectCapabilities.Requires, ProjectCapabilities.VisualC)]

  internal partial class RuleProperties : StronglyTypedPropertyAccess
  {

  ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
  ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
  ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [ImportingConstructor]
    public RuleProperties([Import(RequiredCreationPolicy = CreationPolicy.Shared)] ConfiguredProject configuredProject)
        : base(configuredProject)
    {
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public RuleProperties(ConfiguredProject configuredProject, string file, string itemType, string itemName)
        : base(configuredProject, file, itemType, itemName)
    {
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public RuleProperties(ConfiguredProject configuredProject, IProjectPropertiesContext projectPropertiesContext)
        : base(configuredProject, projectPropertiesContext)
    {
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public RuleProperties(ConfiguredProject configuredProject, UnconfiguredProject unconfiguredProject)
        : base(configuredProject, unconfiguredProject)
    {
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public async Task<Microsoft.Collections.Immutable.IImmutableMap<string, IPropertyPagesCatalog>> ExposeGetNamedCatalogs ()
    {
      return await GetNamedCatalogsAsync ();
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public async Task<Dictionary <string, string>> ProjectPropertiesToDictionary ()
    {
      Dictionary<string, string> evaluatedProperties = new Dictionary<string, string> ();

      var catalogs = await GetNamedCatalogsAsync ();

      var projectPropertyList = catalogs.GetValueOrDefault ("Project");

      IReadOnlyCollection <string> projectPropertySchemas = projectPropertyList.GetPropertyPagesSchemas ();

      foreach (string schema in projectPropertySchemas)
      {
        IRule schemaRules = projectPropertyList.BindToContext (schema, File, ItemType, ItemName);

        foreach (IProperty properties in schemaRules.Properties)
        {
          IEvaluatedProperty evaluatedProperty = (IEvaluatedProperty)properties;

          try
          {
            string schemaGroupedKey = schema + "." + properties.Name;

            evaluatedProperties.Add (schemaGroupedKey, await evaluatedProperty.GetEvaluatedValueAsync ());
          }
          catch (Exception e)
          {
            LoggingUtils.HandleException (e);
          }
        }
      }

      return evaluatedProperties;
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


  }

  ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
  ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
  ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////