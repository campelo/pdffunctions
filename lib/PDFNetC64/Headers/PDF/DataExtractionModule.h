//---------------------------------------------------------------------------------------
// Copyright (c) 2001-2023 by Apryse Software Inc. All Rights Reserved.
// Consult legal.txt regarding legal and license information.
//---------------------------------------------------------------------------------------
// !Warning! This file is autogenerated, modify the .codegen file, not this one
// (any changes here will be wiped out during the autogen process)

#ifndef PDFTRON_H_CPPPDFDataExtractionModule
#define PDFTRON_H_CPPPDFDataExtractionModule
#include <C/PDF/TRN_DataExtractionModule.h>

#include <PDF/DataExtractionOptions.h>
#include <Common/BasicTypes.h>
#include <Common/UString.h>
#include <PDF/PDFDoc.h>

namespace pdftron { namespace PDF { 

/**
 * The class DataExtractionModule.
 * static interface to PDFTron SDKs data extraction functionality
 */
class DataExtractionModule
{
public:
	enum DataExtractionEngine {
		e_Tabular = 0,
		e_Form = 1,
		e_DocStructure = 2
	};
		
	/**
	 * Find out whether the specified data extraction module is available (and licensed).
	 * 
	 * @param engine -- The extraction engine.
	 * @return returns true if data extraction operations can be performed.
	 */
	static bool IsModuleAvailable(DataExtractionEngine engine);
	
	/**
	 * Perform data extraction on a PDF file using the specified engine and return the resulting JSON string.
	 * 
	 * @param input_pdf_file -- The source document filename.
	 * @param engine -- The extraction engine.
	 * @param options -- Data extraction options (optional).
	 * @return JSON string representing the extracted results.
	 */
	static UString ExtractData(const UString& input_pdf_file, DataExtractionEngine engine, DataExtractionOptions* options = 0);
	
	/**
	 * Perform data extraction on a PDF file using the specified engine.
	 * 
	 * @param input_pdf_file -- The source document filename.
	 * @param output_json_file -- The resulting JSON filename.
	 * @param engine -- The extraction engine.
	 * @param options -- Data extraction options (optional).
	 */
	static void ExtractData(const UString& input_pdf_file, const UString& output_json_file, DataExtractionEngine engine, DataExtractionOptions* options = 0);
	
	/**
	 * Perform data extraction on a PDF in XLSX output format.
	 * 
	 * @param input_pdf_file -- The source document filename.
	 * @param output_xlsx_file -- The resulting XLSX filename.
	 * @param options -- Data extraction options (optional).
	 */
	static void ExtractToXLSX(const UString& input_pdf_file, const UString& output_xlsx_file, DataExtractionOptions* options = 0);
	
	/**
	 * Perform data extraction on a PDF in XLSX output format.
	 * 
	 * @param input_pdf_file -- The source document filename.
	 * @param output_xlsx_stream -- The resulting XLSX filter.
	 * @param options -- Data extraction options (optional).
	 */
	static void ExtractToXLSX(const UString& input_pdf_file, Filters::Filter& output_xlsx_stream, DataExtractionOptions* options = 0);

};

#include <Impl/DataExtractionModule.inl>
} //end PDF
} //end pdftron


#endif //PDFTRON_H_CPPPDFDataExtractionModule
