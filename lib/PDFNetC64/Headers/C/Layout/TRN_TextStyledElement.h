//---------------------------------------------------------------------------------------
// Copyright (c) 2001-2023 by Apryse Software Inc. All Rights Reserved.
// Consult legal.txt regarding legal and license information.
//---------------------------------------------------------------------------------------
// !Warning! This file is autogenerated, modify the .codegen file, not this one
// (any changes here will be wiped out during the autogen process)

#ifndef PDFTRON_H_CTextStyledElement
#define PDFTRON_H_CTextStyledElement

#ifdef __cplusplus
extern "C" {
#endif

#include <C/Common/TRN_Types.h>
#include <C/Common/TRN_Exception.h>

TRN_API TRN_TextStyledElementSetFontFace(TRN_TextStyledElement impl, const TRN_UString font_name);
TRN_API TRN_TextStyledElementGetFontFace(TRN_TextStyledElement impl, TRN_UString* result);

TRN_API TRN_TextStyledElementSetFontSize(TRN_TextStyledElement impl, double font_size);
TRN_API TRN_TextStyledElementGetFontSize(TRN_TextStyledElement impl, double* result);

TRN_API TRN_TextStyledElementSetItalic(TRN_TextStyledElement impl, TRN_Bool val);
TRN_API TRN_TextStyledElementIsItalic(TRN_TextStyledElement impl, TRN_Bool* result);

TRN_API TRN_TextStyledElementSetBold(TRN_TextStyledElement impl, TRN_Bool val);
TRN_API TRN_TextStyledElementIsBold(TRN_TextStyledElement impl, TRN_Bool* result);

TRN_API TRN_TextStyledElementSetTextColor(TRN_TextStyledElement impl, TRN_UInt8 red,  TRN_UInt8 green, TRN_UInt8 blue);
TRN_API TRN_TextStyledElementSetBackgroundColor(TRN_TextStyledElement impl, TRN_UInt8 red,  TRN_UInt8 green, TRN_UInt8 blue);

#ifdef __cplusplus
} // extern C
#endif

#endif /* PDFTRON_H_CTextStyledElement */