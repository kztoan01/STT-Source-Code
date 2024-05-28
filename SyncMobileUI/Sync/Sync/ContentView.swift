//
//  ContentView.swift
//  Sync
//
//  Created by Tran Bao Toan on 27/5/24.
//

import SwiftUI

struct ContentView: View {
    var body: some View {
        VStack {
            Image(systemName: "music.note.tv.fill")
                .imageScale(.large)
                .foregroundStyle(.tint)
            Text("Welcome to Sync")
        }
        .padding()
    }
}

#Preview {
    ContentView()
}
