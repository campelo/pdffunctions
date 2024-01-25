//---------------------------------------------------------------------------------------
// Copyright (c) 2001-2023 by Apryse Software Inc. All Rights Reserved.
// Consult legal.txt regarding legal and license information.
//---------------------------------------------------------------------------------------
// !Warning! This file is autogenerated, modify the .codegen file, not this one
// (any changes here will be wiped out during the autogen process)

#ifndef PDFTRON_H_CParagraph
#define PDFTRON_H_CParagraph


#ifdef __cplusplus
extern "C" {
#endif

#include <C/Common/TRN_Types.h>
#include <C/Common/TRN_Exception.h>

enum TRN_ParagraphTextJustification
{
	e_Paragraph_text_justification_invalid = 0,
	e_Paragraph_text_justify_left,
	e_Paragraph_text_justify_right,
	e_Paragraph_text_justify_center
};

TRN_API TRN_ParagraphAddText(TRN_Paragraph self, const TRN_UString text, TRN_TextRun* result);

TRN_API TRN_ParagraphSetSpaceBefore(TRN_Paragraph self, double val);
TRN_API TRN_ParagraphGetSpaceBefore(TRN_Paragraph self, double* result);

TRN_API TRN_ParagraphSetSpaceAfter(TRN_Paragraph self, double val);
TRN_API TRN_ParagraphGetSpaceAfter(TRN_Paragraph self, double* result);

TRN_API TRN_ParagraphSetJustificationMode(TRN_Paragraph self, enum TRN_ParagraphTextJustification val);
TRN_API TRN_ParagraphGetJustificationMode(TRN_Paragraph self, enum TRN_ParagraphTextJustification* result);

TRN_API TRN_ParagraphSetStartIndent(TRN_Paragraph self, double val);
TRN_API TRN_ParagraphGetStartIndent(TRN_Paragraph self, double* result);
TRN_API TRN_ParagraphSetEndIndent(TRN_Paragraph self, double val);
TRN_API TRN_ParagraphGetEndIndent(TRN_Paragraph self, double* result);
TRN_API TRN_ParagraphSetTextIndent(TRN_Paragraph self, double val);
TRN_API TRN_ParagraphGetTextIndent(TRN_Paragraph self, double* result);

TRN_API TRN_ParagraphSetBorder(TRN_Paragraph self, double thickness, TRN_UInt8 red, TRN_UInt8 green, TRN_UInt8 blue);
TRN_API TRN_ParagraphGetBorderThickness(TRN_Paragraph self, double* result);

TRN_API TRN_ParagraphAddTabStop(TRN_Paragraph self, double val);
TRN_API TRN_ParagraphGetNextTabStop(TRN_Paragraph self, double val, double* result);
TRN_API TRN_ParagraphSetDefaultTabStop(TRN_Paragraph self, double val);
TRN_API TRN_ParagraphGetDefaultTabStop(TRN_Paragraph self, double* result);
TRN_API TRN_ParagraphSetSpacesPerTab(TRN_Paragraph self, TRN_UInt32 val);
TRN_API TRN_ParagraphGetSpacesPerTab(TRN_Paragraph self, TRN_UInt32* result);

TRN_API TRN_ParagraphSetDisplayRtl(TRN_Paragraph self, TRN_Bool val);
TRN_API TRN_ParagraphIsDisplayRtl(TRN_Paragraph self, TRN_Bool* result);

#ifdef __cplusplus
} // extern C
#endif

#endif /* PDFTRON_H_CParagraph */