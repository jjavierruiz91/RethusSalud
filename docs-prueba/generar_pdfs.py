# -*- coding: utf-8 -*-
"""Genera 4 PDFs de prueba/demo para cargar en el modulo de documentos de RethusSalud."""

from reportlab.lib.pagesizes import letter
from reportlab.lib.units import cm
from reportlab.lib import colors
from reportlab.pdfgen import canvas
from reportlab.lib.utils import ImageReader
import os

OUT_DIR = os.path.dirname(os.path.abspath(__file__))

NOMBRE = "Jose Javier Ruiz Mendoza"
CEDULA = "1.065.628.638"
UNIVERSIDAD = "Universidad Popular del Cesar"
PROFESION = "Psicologia"


def marca_agua(c, width, height):
    c.saveState()
    c.setFont("Helvetica-Bold", 40)
    c.setFillColor(colors.Color(0.85, 0.1, 0.1, alpha=0.25))
    c.translate(width / 2, height / 2)
    c.rotate(45)
    c.drawCentredString(0, 0, "DOCUMENTO DE PRUEBA")
    c.drawCentredString(0, -50, "USO DEMOSTRATIVO")
    c.restoreState()


def encabezado(c, width, height, titulo):
    c.setFillColor(colors.HexColor("#0F5FA6"))
    c.rect(0, height - 2.2 * cm, width, 2.2 * cm, fill=True, stroke=False)
    c.setFillColor(colors.white)
    c.setFont("Helvetica-Bold", 14)
    c.drawCentredString(width / 2, height - 1.0 * cm, "SECRETARIA DE SALUD DEPARTAMENTAL DEL CESAR")
    c.setFont("Helvetica", 10)
    c.drawCentredString(width / 2, height - 1.6 * cm, titulo)
    c.setFillColor(colors.black)


def pie(c, width):
    c.setFont("Helvetica-Oblique", 8)
    c.setFillColor(colors.grey)
    c.drawCentredString(width / 2, 1.3 * cm,
                         "Este documento es un archivo de prueba generado unicamente para validar cargas en el portal Rethus.")
    c.drawCentredString(width / 2, 1.0 * cm, "No tiene validez legal ni representa un documento oficial real.")
    c.setFillColor(colors.black)


def crear_cedula(path):
    width, height = letter
    c = canvas.Canvas(path, pagesize=letter)
    marca_agua(c, width, height)
    encabezado(c, width, height, "SIMULACRO - CEDULA DE CIUDADANIA AMPLIADA AL 150%")

    y = height - 4.0 * cm
    c.setFont("Helvetica-Bold", 12)
    c.drawString(3 * cm, y, "REPUBLICA DE COLOMBIA")
    c.setFont("Helvetica", 10)
    c.drawString(3 * cm, y - 0.5 * cm, "IDENTIFICACION PERSONAL")

    c.setLineWidth(1)
    c.rect(3 * cm, y - 6.5 * cm, width - 6 * cm, 5.5 * cm)

    c.rect(3.3 * cm, y - 6.0 * cm, 3.5 * cm, 4.5 * cm)
    c.setFont("Helvetica", 8)
    c.drawCentredString(3.3 * cm + 1.75 * cm, y - 3.9 * cm, "FOTOGRAFIA")
    c.drawCentredString(3.3 * cm + 1.75 * cm, y - 4.2 * cm, "(simulada)")

    campos = [
        ("Apellidos y nombres", NOMBRE.upper()),
        ("Numero", CEDULA),
        ("Fecha de nacimiento", "15 de marzo de 1991"),
        ("Lugar de nacimiento", "Valledupar, Cesar"),
        ("Estatura", "1.75 m"),
        ("G.S. - R.H.", "O+"),
        ("Fecha y lugar de expedicion", "20 de marzo de 2009 - Valledupar, Cesar"),
    ]
    ty = y - 0.8 * cm
    tx = 3.3 * cm + 4.0 * cm
    c.setFont("Helvetica-Bold", 9)
    for etiqueta, valor in campos:
        c.setFont("Helvetica", 8)
        c.drawString(tx, ty, etiqueta.upper())
        c.setFont("Helvetica-Bold", 10)
        c.drawString(tx, ty - 0.4 * cm, valor)
        ty -= 1.0 * cm

    pie(c, width)
    c.showPage()
    c.save()


def crear_diploma(path):
    width, height = letter
    c = canvas.Canvas(path, pagesize=letter)
    marca_agua(c, width, height)
    encabezado(c, width, height, "DIPLOMA DE GRADO (documento de prueba)")

    c.setLineWidth(2)
    c.setStrokeColor(colors.HexColor("#0F5FA6"))
    c.rect(1.5 * cm, 2.2 * cm, width - 3 * cm, height - 5 * cm)

    c.setFillColor(colors.black)
    c.setFont("Helvetica-Bold", 20)
    c.drawCentredString(width / 2, height - 5.5 * cm, UNIVERSIDAD.upper())
    c.setFont("Helvetica", 12)
    c.drawCentredString(width / 2, height - 6.3 * cm, "Otorga el presente")
    c.setFont("Helvetica-Bold", 16)
    c.drawCentredString(width / 2, height - 7.2 * cm, "DIPLOMA DE GRADO")

    c.setFont("Helvetica", 11)
    c.drawCentredString(width / 2, height - 9.0 * cm, "Por medio del presente documento se certifica que")
    c.setFont("Helvetica-Bold", 15)
    c.drawCentredString(width / 2, height - 10.0 * cm, NOMBRE)
    c.setFont("Helvetica", 11)
    c.drawCentredString(width / 2, height - 10.7 * cm, f"identificado(a) con Cedula de Ciudadania No. {CEDULA}")
    c.drawCentredString(width / 2, height - 11.8 * cm, "ha cumplido con todos los requisitos academicos exigidos por el programa de")
    c.setFont("Helvetica-Bold", 13)
    c.drawCentredString(width / 2, height - 12.6 * cm, PROFESION.upper())
    c.setFont("Helvetica", 11)
    c.drawCentredString(width / 2, height - 13.3 * cm, "por lo cual se le confiere el titulo correspondiente.")

    c.setFont("Helvetica", 10)
    c.drawCentredString(width / 2, height - 15.5 * cm, "Dado en Valledupar, Cesar, a los 28 dias del mes de noviembre de 2015.")

    c.line(3.5 * cm, 4.0 * cm, 9.5 * cm, 4.0 * cm)
    c.drawCentredString(6.5 * cm, 3.6 * cm, "Rector")
    c.line(width - 9.5 * cm, 4.0 * cm, width - 3.5 * cm, 4.0 * cm)
    c.drawCentredString(width - 6.5 * cm, 3.6 * cm, "Secretario General")

    pie(c, width)
    c.showPage()
    c.save()


def crear_acta(path):
    width, height = letter
    c = canvas.Canvas(path, pagesize=letter)
    marca_agua(c, width, height)
    encabezado(c, width, height, "ACTA DE GRADO (documento de prueba)")

    c.setFont("Helvetica-Bold", 13)
    c.drawCentredString(width / 2, height - 3.5 * cm, UNIVERSIDAD.upper())
    c.setFont("Helvetica-Bold", 12)
    c.drawCentredString(width / 2, height - 4.3 * cm, "ACTA DE GRADO No. AG-2015-0742")

    texto = [
        "En la ciudad de Valledupar, Cesar, siendo las 10:00 a.m. del dia 28 de noviembre de 2015,",
        f"se reunio el Consejo Academico de la {UNIVERSIDAD} con el fin de otorgar el grado",
        f"correspondiente al programa de {PROFESION}.",
        "",
        f"Verificado el cumplimiento de los requisitos academicos y administrativos, se deja constancia",
        f"de que el(la) estudiante {NOMBRE}, identificado(a) con Cedula de Ciudadania",
        f"No. {CEDULA}, cumplio satisfactoriamente con el plan de estudios y demas",
        "requisitos de grado establecidos por la institucion.",
        "",
        "En constancia de lo anterior, se firma la presente acta por los integrantes del Consejo",
        "Academico.",
    ]
    ty = height - 5.6 * cm
    c.setFont("Helvetica", 10)
    for linea in texto:
        c.drawString(2.5 * cm, ty, linea)
        ty -= 0.65 * cm

    ty -= 1 * cm
    for cargo in ["Rector", "Secretario General", "Decano de Facultad"]:
        c.line(2.5 * cm, ty, 8.5 * cm, ty)
        c.drawString(2.5 * cm, ty - 0.4 * cm, cargo)
        ty -= 1.5 * cm

    pie(c, width)
    c.showPage()
    c.save()


def crear_tarjeta_profesional(path):
    width, height = letter
    c = canvas.Canvas(path, pagesize=letter)
    marca_agua(c, width, height)
    encabezado(c, width, height, "TARJETA PROFESIONAL (documento de prueba)")

    card_x, card_y = 3 * cm, height - 10.5 * cm
    card_w, card_h = width - 6 * cm, 5.5 * cm

    c.setFillColor(colors.HexColor("#0F5FA6"))
    c.roundRect(card_x, card_y, card_w, card_h, 0.3 * cm, fill=True, stroke=False)
    c.setFillColor(colors.white)
    c.roundRect(card_x + 0.2 * cm, card_y + 0.2 * cm, card_w - 0.4 * cm, card_h - 0.4 * cm, 0.25 * cm, fill=True, stroke=False)

    c.setFillColor(colors.HexColor("#0F5FA6"))
    c.setFont("Helvetica-Bold", 11)
    c.drawString(card_x + 0.7 * cm, card_y + card_h - 0.9 * cm, "TARJETA PROFESIONAL")
    c.setFont("Helvetica", 8)
    c.drawString(card_x + 0.7 * cm, card_y + card_h - 1.3 * cm, "Secretaria de Salud Departamental del Cesar")

    c.setFillColor(colors.black)
    c.rect(card_x + 0.7 * cm, card_y + 1.0 * cm, 2.8 * cm, 3.2 * cm, fill=False)
    c.setFont("Helvetica", 7)
    c.drawCentredString(card_x + 0.7 * cm + 1.4 * cm, card_y + 2.6 * cm, "FOTOGRAFIA")
    c.drawCentredString(card_x + 0.7 * cm + 1.4 * cm, card_y + 2.3 * cm, "(simulada)")

    campos = [
        ("Nombre", NOMBRE),
        ("Cedula de ciudadania", CEDULA),
        ("Profesion", PROFESION),
        ("No. Tarjeta profesional", "TP-CESAR-2016-004512"),
        ("Fecha de expedicion", "10 de febrero de 2016"),
        ("Entidad emisora", "Secretaria de Salud Departamental del Cesar"),
    ]
    ty = card_y + card_h - 1.9 * cm
    tx = card_x + 3.9 * cm
    for etiqueta, valor in campos:
        c.setFont("Helvetica", 7)
        c.drawString(tx, ty, etiqueta.upper())
        c.setFont("Helvetica-Bold", 9)
        c.drawString(tx, ty - 0.35 * cm, valor)
        ty -= 0.85 * cm

    pie(c, width)
    c.showPage()
    c.save()


if __name__ == "__main__":
    archivos = {
        "Cedula ampliada al 150%.pdf": crear_cedula,
        "Diploma de grado.pdf": crear_diploma,
        "Acta de grado.pdf": crear_acta,
        "Tarjeta profesional.pdf": crear_tarjeta_profesional,
    }
    for nombre_archivo, funcion in archivos.items():
        ruta = os.path.join(OUT_DIR, nombre_archivo)
        funcion(ruta)
        print(f"Generado: {ruta}")
