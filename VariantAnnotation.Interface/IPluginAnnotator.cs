﻿using System.Collections.Generic;
using VariantAnnotation.Interface.AnnotatedPositions;
using VariantAnnotation.Interface.GeneAnnotation;
using VariantAnnotation.Interface.Positions;
using VariantAnnotation.Interface.Providers;
using VariantAnnotation.Interface.Sequence;

namespace VariantAnnotation.Interface
{
    public interface IPluginAnnotator
    {
        GenomeAssembly GenomeAssembly { get; }
        IDataSourceVersion DataSourceVersion { get; }
        void Annotate(IAnnotatedPosition annotatedPosition, ISequence sequence);
    }
}