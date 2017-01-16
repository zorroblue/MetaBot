from django.shortcuts import render
from django.http import HttpResponse,JsonResponse
from meta_scripy import get_how_to
import json

def cosine(request,string):
    #code
    return JsonResponse(get_how_to(string),safe = False)

    
