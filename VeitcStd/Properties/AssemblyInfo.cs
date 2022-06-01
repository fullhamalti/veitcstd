using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#if VETIC_HAVE_SIMIFACE
using Sims3.SimIFace;
#endif

[assembly: AssemblyTitle("veitc::std")]
[assembly: AssemblyCompany("Veitc")]
[assembly: AssemblyProduct("libcs (VeitcStd) for Sims 3, Sims Medieval")]
[assembly: AssemblyCopyright("Copyright 2018-2022 Fullham Alfayet")]

[assembly: Guid("d5d4934e-ae98-4881-b9cf-ff448ec1d09a")]

#if VETIC_HAVE_SIMIFACE
[assembly: PersistableStatic]
[assembly: Tunable]
#endif