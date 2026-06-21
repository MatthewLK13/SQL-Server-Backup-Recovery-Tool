import zipfile
import xml.etree.ElementTree as ET
import sys

def read_docx(file_path):
    try:
        with zipfile.ZipFile(file_path, 'r') as docx:
            xml_content = docx.read('word/document.xml')
            tree = ET.XML(xml_content)
            
            # The namespace for Word processing XML
            WORD_NAMESPACE = '{http://schemas.openxmlformats.org/wordprocessingml/2006/main}'
            PARA = WORD_NAMESPACE + 'p'
            TEXT = WORD_NAMESPACE + 't'
            
            paragraphs = []
            for paragraph in tree.iter(PARA):
                texts = [node.text for node in paragraph.iter(TEXT) if node.text]
                if texts:
                    paragraphs.append(''.join(texts))
            
            output = '\n'.join(paragraphs)
            sys.stdout.buffer.write(output.encode('utf-8'))
    except Exception as e:
        sys.stderr.buffer.write(f"Error: {e}\n".encode('utf-8'))

if __name__ == "__main__":
    if len(sys.argv) > 1:
        read_docx(sys.argv[1])
